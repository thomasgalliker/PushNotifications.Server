using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Http;
using Newtonsoft.Json;
using PushNotifications.Server.Internals;
using PushNotifications.Server.Logging;

namespace PushNotifications.Server.Google
{
    /// <summary>
    /// Sends push messages to Firebase Cloud Messaging using the FCM v1 HTTP API.
    /// </summary>
    [DebuggerDisplay("FcmClient: {FcmClient.ApiName}")]
    public class FcmClient : IFcmClient
    {
        public const string ApiName = "FCM HTTP v1 API";

        private readonly ILogger logger;
        private readonly HttpClient httpClient;
        private readonly ServiceAccountCredential credential;
        private readonly string projectId;

        private FcmClient(ILogger logger, HttpClient httpClient)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));

            this.httpClient.DefaultRequestHeaders.UserAgent.Clear();
            this.httpClient.DefaultRequestHeaders.UserAgent.Add(HttpClientUtils.GetProductInfo(this));
        }

        public FcmClient(FcmOptions options)
            : this(Logger.Current, new HttpClient(), options)
        {
        }

        public FcmClient(ILogger logger, HttpClient httpClient, FcmOptions options)
            : this(logger, httpClient)
        {
            if (logger is null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            if (httpClient is null)
            {
                throw new ArgumentNullException(nameof(httpClient));
            }

            if (options is null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (string.IsNullOrEmpty(options.ServiceAccountKeyFilePath))
            {
                throw new ArgumentException($"ServiceAccountKeyFilePath must not be null or empty", $"{nameof(options)}.{nameof(options.ServiceAccountKeyFilePath)}");
            }

            var fileInfo = new FileInfo(options.ServiceAccountKeyFilePath);
            if (fileInfo.Exists)
            {
                var serviceAccountKeyFileContent = File.ReadAllText(options.ServiceAccountKeyFilePath);
                this.credential = this.CreateServiceAccountCredential(new HttpClientFactory(), serviceAccountKeyFileContent);
                this.projectId = GetProjectId(serviceAccountKeyFileContent);
            }
            else
            {
                throw new FileNotFoundException($"ServiceAccountKeyFilePath could not be found at {fileInfo.FullName}");
            }
        }

        private static string GetProjectId(string serviceAccountKeyFileContent)
        {
            var jsonCredentialParameters = JsonConvert.DeserializeObject<JsonCredentialParameters>(serviceAccountKeyFileContent);
            if (jsonCredentialParameters != null)
            {
                return jsonCredentialParameters.ProjectId;
            }

            throw new Exception($"Could not read project_id from ServiceAccountKeyFilePath");
        }

        private ServiceAccountCredential CreateServiceAccountCredential(HttpClientFactory httpClientFactory, string credentials)
        {
            var serviceAccountCredential = GoogleCredential.FromJson(credentials)
                .CreateScoped("https://www.googleapis.com/auth/firebase.messaging")
                .UnderlyingCredential as ServiceAccountCredential;

            if (serviceAccountCredential == null)
            {
                throw new InvalidOperationException($"Error creating ServiceAccountCredential");
            }

            var initializer = new ServiceAccountCredential.Initializer(serviceAccountCredential.Id, serviceAccountCredential.TokenServerUrl)
            {
                User = serviceAccountCredential.User,
                AccessMethod = serviceAccountCredential.AccessMethod,
                Clock = serviceAccountCredential.Clock,
                Key = serviceAccountCredential.Key,
                Scopes = serviceAccountCredential.Scopes,
                HttpClientFactory = httpClientFactory
            };

            return new ServiceAccountCredential(initializer);
        }

        private async Task<string> CreateAccessTokenAsync(CancellationToken cancellationToken)
        {
            // Execute the Request:
            var accessToken = await this.credential.GetAccessTokenForRequestAsync(cancellationToken: cancellationToken).ConfigureAwait(false);
            if (accessToken == null)
            {
                throw new InvalidOperationException("Failed to get access token for request");
            }

            return accessToken;
        }

        public async Task<FcmResponse> SendAsync(FcmRequest fcmRequest, CancellationToken ct = default)
        {
            if (fcmRequest == null)
            {
                throw new ArgumentNullException(nameof(fcmRequest));
            }

            if (fcmRequest.Message == null)
            {
                throw new ArgumentNullException($"{nameof(fcmRequest)}.{nameof(fcmRequest.Message)}");
            }

            var url = $"https://fcm.googleapis.com/v1/projects/{this.projectId}/messages:send";
            var request = new HttpRequestMessage(HttpMethod.Post, url);

            var accessToken = await this.CreateAccessTokenAsync(ct).ConfigureAwait(false);
            request.Headers.TryAddWithoutValidation("Authorization", $"Bearer {accessToken}");

            var payload = JsonConvert.SerializeObject(fcmRequest, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            request.Content = new JsonContent(payload);

            var response = await this.httpClient.SendAsync(request, ct).ConfigureAwait(false);

            var responseContentJson = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            this.logger.Log(LogLevel.Debug, $"SendAsync returned json content:{Environment.NewLine}{responseContentJson}");

            var tokenDebuggerDisplay = $"Token={fcmRequest.Message.Token ?? "null"}";

            var fcmResponse = JsonConvert.DeserializeObject<FcmResponse>(responseContentJson);
            fcmResponse.Token = fcmRequest.Message.Token;  // Assign registration ID to each result in the list

            if (response.StatusCode == HttpStatusCode.OK) // TODO Use if (response.IsSuccessStatusCode)
            {
                this.logger.Log(LogLevel.Info, $"SendAsync to {tokenDebuggerDisplay} successfully completed");
                return fcmResponse;
            }
            else
            {
                this.logger.Log(LogLevel.Error, $"SendAsync to {tokenDebuggerDisplay} failed with StatusCode={(int)response.StatusCode} ({response.StatusCode})");
                return fcmResponse;
            }
        }
    }
}
