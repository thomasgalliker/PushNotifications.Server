﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PushNotifications.Server.Internals;
using PushNotifications.Server.Logging;

namespace PushNotifications.Server.Google.Legacy
{
    /// <summary>
    /// Sends push messages to Firebase Cloud Messaging using the FCM legacy HTTP API.
    /// </summary>
    [DebuggerDisplay("FcmClient: {FcmClient.ApiName}")]
    public class FcmClient : IFcmClient
    {
        public const string ApiName = "FCM legacy HTTP API";

        private readonly ILogger logger;
        private readonly HttpClient httpClient;
        private readonly FcmOptions options;

        /// <summary>
        /// Constructs a client instance with given <paramref name="options"/>
        /// for token-based authentication (using a .p8 certificate).
        /// </summary>
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

            this.options = options ?? throw new ArgumentNullException(nameof(options));

            this.httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "key=" + this.options.SenderAuthToken);
        }

        public async Task<FcmResponse> SendAsync(FcmRequest fcmRequest, CancellationToken ct = default)
        {
            if (fcmRequest == null)
            {
                throw new ArgumentNullException(nameof(fcmRequest));
            }

            // If 'To' was used instead of RegistrationIds, let's make RegistrationId's null
            // so we don't serialize an empty array for this property
            // otherwise, google will complain that we specified both instead
            if (fcmRequest.RegistrationIds != null && fcmRequest.RegistrationIds.Count <= 0 && !string.IsNullOrEmpty(fcmRequest.To))
            {
                fcmRequest.RegistrationIds = null;
            }

            var url = "https://fcm.googleapis.com/fcm/send";
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            var payload = JsonConvert.SerializeObject(fcmRequest, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            request.Content = new JsonContent(payload);

            var response = await this.httpClient.SendAsync(request, ct).ConfigureAwait(false);

            var responseContentJson = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            this.logger.Log(LogLevel.Debug, $"SendAsync returned json content:{Environment.NewLine}{responseContentJson}");

            var tokenDebuggerDisplay = fcmRequest.RegistrationIds?.Count > 0 ? $"RegistrationIds=[{string.Join(", ", fcmRequest.RegistrationIds)}]" : ($"To={fcmRequest.To ?? "null"}");
           
            var fcmResponse = JsonConvert.DeserializeObject<FcmResponse>(responseContentJson);

            // Assign registration ID to each result in the list
            fcmResponse.Results.ForPair(fcmRequest.RegistrationIds ?? new List<string> { fcmRequest.To }, (r, id) => r.RegistrationId = id);

            if (response.StatusCode == HttpStatusCode.OK) // TODO Use if (response.IsSuccessStatusCode)
            {
                this.logger.Log(LogLevel.Info, $"SendAsync to {tokenDebuggerDisplay} successfully completed");
            }
            else
            {
                this.logger.Log(LogLevel.Error, $"SendAsync to {tokenDebuggerDisplay} failed with StatusCode={(int)response.StatusCode} ({response.StatusCode})");
            }

            return fcmResponse;
        }
    }
}
