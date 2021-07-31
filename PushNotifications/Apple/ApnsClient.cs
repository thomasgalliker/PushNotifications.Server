using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PushNotifications.Abstractions;
using PushNotifications.Internals;
using PushNotifications.Logging;

namespace PushNotifications.Apple
{
    public partial class ApnsClient : IApnsClient
    {
        internal const string DevelopmentEndpoint = "https://api.development.push.apple.com";   // --> Sandbox
        internal const string ProductionEndpoint = "https://api.push.apple.com";                // --> Production
        private const string bundleIdVoipSuffix = ".voip";

        private readonly ILogger logger;
        private readonly HttpClient httpClient;
        private readonly CngKey key;
        private readonly string keyId;
        private readonly string teamId;
        private readonly string bundleId;
        private readonly bool useCert;
        private readonly bool isVoipCert;

        private string jwt;
        private DateTime lastJwtGenerationTime;
        private readonly object jwtRefreshLock = new object();

        private bool useSandbox;
        private bool useBackupPort;

        /// <summary>
        /// Constructs a client instance with given <paramref name="options"/>
        /// for token-based authentication (using a .p8 certificate).
        /// </summary>
        private ApnsClient(ILogger logger, HttpClient httpClient)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));

            this.httpClient.DefaultRequestHeaders.UserAgent.Clear();
            this.httpClient.DefaultRequestHeaders.UserAgent.Add(HttpClientUtils.GetProductInfo(this));
        }

        public ApnsClient(ApnsJwtOptions options)
            : this(Logger.Current, new HttpClient(), options)
        {
        }

        public ApnsClient(X509Certificate x509Certificate)
            : this(Logger.Current, new HttpClient(), x509Certificate)
        {
        }

        public ApnsClient(ILogger logger, HttpClient httpClient, ApnsJwtOptions options)
            : this(logger, httpClient)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            string certContent;
            if (options.CertFilePath != null)
            {
                var fileInfo = new FileInfo(options.CertFilePath);
                if (fileInfo.Exists)
                {
                    certContent = File.ReadAllText(options.CertFilePath);
                }
                else
                {
                    throw new FileNotFoundException($"Certificate file (CertFilePath) could not be found at: {fileInfo.FullName}");
                }
            }
            else if (options.CertContent != null)
            {
                certContent = options.CertContent;
            }
            else
            {
                throw new ArgumentException("Either certificate file path or certificate contents must be provided.", nameof(options));
            }

            certContent = certContent
                .Replace("\r", "")
                .Replace("\n", "")
                .Replace("-----BEGIN PRIVATE KEY-----", "")
                .Replace("-----END PRIVATE KEY-----", "");

            var key = CngKey.Import(Convert.FromBase64String(certContent), CngKeyBlobFormat.Pkcs8PrivateBlob);

            this.key = key ?? throw new ArgumentNullException(nameof(key));

            this.keyId = options.KeyId ?? throw new ArgumentNullException(nameof(options.KeyId),
                $"Make sure {nameof(ApnsJwtOptions)}.{nameof(options.KeyId)} is set to a non-null value.");

            this.teamId = options.TeamId ?? throw new ArgumentNullException(nameof(options.TeamId),
                $"Make sure {nameof(ApnsJwtOptions)}.{nameof(options.TeamId)} is set to a non-null value.");

            this.bundleId = options.BundleId ?? throw new ArgumentNullException(nameof(options.BundleId),
                $"Make sure {nameof(ApnsJwtOptions)}.{nameof(options.BundleId)} is set to a non-null value.");

            this.useSandbox = options.UseSandbox;
        }

        /// <summary>
        /// Constructs a client instance with given <paramref name="x509Certificate"/>
        /// for certificate-based authentication (using a .p12 certificate).
        /// </summary>
        public ApnsClient(ILogger logger, HttpClient httpClient, X509Certificate x509Certificate)
            : this(logger, httpClient)
        {
            if (x509Certificate == null)
            {
                throw new ArgumentNullException(nameof(x509Certificate));
            }

            var split = x509Certificate.Subject.Split(new[] { "0.9.2342.19200300.100.1.1=" }, StringSplitOptions.RemoveEmptyEntries);
            if (split.Length != 2)
            {
                // On Linux .NET Core cert.Subject prints `userId=xxx` instead of `0.9.2342.19200300.100.1.1=xxx`
                split = x509Certificate.Subject.Split(new[] { "userId=" }, StringSplitOptions.RemoveEmptyEntries);
            }

            if (split.Length != 2)
            {
                // if subject prints `uid=xxx` instead of `0.9.2342.19200300.100.1.1=xxx`
                split = x509Certificate.Subject.Split(new[] { "uid=" }, StringSplitOptions.RemoveEmptyEntries);
            }

            if (split.Length != 2)
            {
                throw new InvalidOperationException("Provided certificate does not appear to be a valid APNs certificate.");
            }

            var topic = split[1];
            this.isVoipCert = topic.EndsWith(bundleIdVoipSuffix);
            this.bundleId = split[1].Replace(bundleIdVoipSuffix, "");
            this.useCert = true;
        }

        public async Task<ApnsResponse> SendAsync(ApnsRequest apnsRequest, CancellationToken ct = default)
        {
            this.logger.Log(LogLevel.Debug, $"SendAsync to Token={apnsRequest.Token} started...");

            if (this.useCert)
            {
                if (this.isVoipCert && apnsRequest.Type != ApplePushType.Voip)
                {
                    throw new InvalidOperationException("Provided certificate can only be used to send 'voip' type pushes.");
                }
            }

            var payload = apnsRequest.GeneratePayload();

            var token = (apnsRequest.Token ?? apnsRequest.VoipToken);
            string url = (this.useSandbox ? DevelopmentEndpoint : ProductionEndpoint)
                + (this.useBackupPort ? ":2197" : ":443")
                + "/3/device/"
                + token;
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Version = new Version(2, 0);
            request.Headers.Add("apns-priority", apnsRequest.Priority.ToString());
            request.Headers.Add("apns-push-type", apnsRequest.Type.ToString().ToLowerInvariant());
            request.Headers.Add("apns-topic", this.GetTopic(apnsRequest.Type));

            if (!this.useCert)
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("bearer", this.GetOrGenerateJwt());
            }

            if (apnsRequest.Expiration is DateTimeOffset expiration)
            {
                var expirationValue = expiration == DateTimeOffset.MinValue ? 0L : expiration.ToUnixTimeSeconds();
                request.Headers.Add("apns-expiration", $"{expirationValue}");
            }

            if (!string.IsNullOrEmpty(apnsRequest.CollapseId))
            {
                request.Headers.Add("apns-collapse-id", apnsRequest.CollapseId);
            }

            request.Content = new JsonContent(payload);

            HttpResponseMessage response;
            try
            {
                response = await this.httpClient.SendAsync(request, ct).ConfigureAwait(false);
            }
            catch (HttpRequestException ex) when (
                Environment.OSVersion.Platform is PlatformID.Win32NT &&
                ex.InnerException is AuthenticationException { InnerException: Win32Exception { NativeErrorCode: -2146893016 } } ||
                Environment.OSVersion.Platform is PlatformID.Unix &&
                ex.InnerException is IOException { InnerException: IOException { InnerException: IOException { InnerException: { InnerException: { HResult: 336151573 } } } } })
            {
                throw new ApnsCertificateExpiredException(ex);
            }

            var responseContentJson = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            this.logger.Log(LogLevel.Debug, $"SendAsync returned json content:{Environment.NewLine}{responseContentJson}");

            // Process status codes specified by APNs documentation
            // https://developer.apple.com/documentation/usernotifications/setting_up_a_remote_notification_server/handling_notification_responses_from_apns
            if (response.StatusCode == HttpStatusCode.OK)
            {
                this.logger.Log(LogLevel.Info, $"SendAsync to Token={apnsRequest.Token} successfully completed");
                return ApnsResponse.Successful(apnsRequest.Token);
            }

            var errorPayload = JsonConvert.DeserializeObject<ApnsErrorResponsePayload>(responseContentJson);
            this.logger.Log(LogLevel.Error, $"SendAsync to Token={apnsRequest.Token} failed with StatusCode={(int)response.StatusCode}({response.StatusCode}), Reason={errorPayload.Reason}");

            return ApnsResponse.Error(apnsRequest.Token, errorPayload.Reason);
        }

        public static ApnsClient CreateUsingCert(X509Certificate2 cert)
        {
#if NETSTANDARD2_0
            throw new NotSupportedException("Certificate-based connection is not supported on all .NET Framework versions and on .NET Core 2.x or lower.");
#elif NETSTANDARD2_1
            if (cert == null) throw new ArgumentNullException(nameof(cert));

            var handler = new HttpClientHandler();
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;

            handler.ClientCertificates.Add(cert);
            var httpClient = new HttpClient(handler);
            return new ApnsClient(Logger.Current, httpClient, cert);
#endif


        }

        public static ApnsClient CreateUsingCert(string pathToCert, string certPassword = null)
        {
            if (string.IsNullOrWhiteSpace(pathToCert))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(pathToCert));
            }

            var cert = new X509Certificate2(pathToCert, certPassword);
            return CreateUsingCert(cert);
        }

        public ApnsClient UseSandbox()
        {
            this.useSandbox = true;
            return this;
        }

        /// <summary>
        /// Use port 2197 instead of 443 to connect to the APNs server.
        /// You might use this port to allow APNs traffic through your firewall but to block other HTTPS traffic.
        /// </summary>
        /// <returns></returns>
        public ApnsClient UseBackupPort()
        {
            this.useBackupPort = true;
            return this;
        }

        private string GetTopic(ApplePushType pushType)
        {
            switch (pushType)
            {
                case ApplePushType.Background:
                case ApplePushType.Alert:
                    return this.bundleId;
                case ApplePushType.Voip:
                    return this.bundleId + bundleIdVoipSuffix;
                case ApplePushType.Unknown:
                default:
                    throw new ArgumentOutOfRangeException(nameof(pushType), pushType, null);
            }
        }

        private string GetOrGenerateJwt()
        {
            lock (this.jwtRefreshLock)
            {
                if (this.lastJwtGenerationTime > DateTime.UtcNow - TimeSpan.FromMinutes(20)) // refresh no more than once every 20 minutes
                {
                    return this.jwt;
                }

                var now = DateTimeOffset.UtcNow;

                string header = JsonConvert.SerializeObject(new { alg = "ES256", kid = this.keyId });
                string payload = JsonConvert.SerializeObject(new { iss = this.teamId, iat = now.ToUnixTimeSeconds() });

                string headerBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(header));
                string payloadBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(payload));
                string unsignedJwtData = $"{headerBase64}.{payloadBase64}";

                byte[] signature;
                using (var dsa = new ECDsaCng(this.key))
                {
                    dsa.HashAlgorithm = CngAlgorithm.Sha256;
                    signature = dsa.SignData(Encoding.UTF8.GetBytes(unsignedJwtData));
                }

                this.jwt = $"{unsignedJwtData}.{Convert.ToBase64String(signature)}";
                this.lastJwtGenerationTime = now.UtcDateTime;
                return this.jwt;
            }
        }
    }
}