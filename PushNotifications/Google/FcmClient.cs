using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PushNotifications.Abstractions;
using PushNotifications.Internals;
using PushNotifications.Logging;

namespace PushNotifications.Google
{
    public class FcmClient : IFcmClient
    {
        private readonly HttpClient httpClient;
        private readonly FcmConfiguration configuration;

        public FcmClient(FcmConfiguration configuration) : this(new HttpClient(), configuration)
        {
        }

        public FcmClient(HttpClient httpClient, FcmConfiguration configuration)
        {
            this.httpClient = httpClient;
            this.configuration = configuration;

            this.httpClient.DefaultRequestHeaders.UserAgent.Clear();
            this.httpClient.DefaultRequestHeaders.UserAgent.Add(HttpClientUtils.GetProductInfo(this));
            this.httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "key=" + this.configuration.SenderAuthToken);
        }

        public async Task<FcmResponse> SendAsync(FcmRequest fcmRequest, CancellationToken ct = default)
        {
            // If 'To' was used instead of RegistrationIds, let's make RegistrationId's null
            // so we don't serialize an empty array for this property
            // otherwise, google will complain that we specified both instead
            if (fcmRequest.RegistrationIds != null && fcmRequest.RegistrationIds.Count <= 0 && !string.IsNullOrEmpty(fcmRequest.To))
            {
                fcmRequest.RegistrationIds = null;
            }

            var url = this.configuration.FcmUrl;
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            var payload = JsonConvert.SerializeObject(fcmRequest, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            request.Content = new JsonContent(payload);

            var response = await this.httpClient.SendAsync(request, ct).ConfigureAwait(false);

            var responseContentJson = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            Logger.Debug($"SendAsync returned json content:{Environment.NewLine}{responseContentJson}");

            if (response.StatusCode == HttpStatusCode.OK) // TODO Use if (response.IsSuccessStatusCode)
            {
                Logger.Info($"SendAsync successfully completed");
                var fcmResponse = JsonConvert.DeserializeObject<FcmResponse>(responseContentJson);

                // Assign registration ID to each result in the list
                fcmResponse.Results.ForPair(fcmRequest.RegistrationIds ?? new List<string> { fcmRequest.To }, (r, id) => r.RegistrationId = id);
                
                return fcmResponse;
            }

            if ((int)response.StatusCode >= 500 && (int)response.StatusCode < 600)
            {
                //First try grabbing the retry-after header and parsing it.
                var retryAfterHeader = response.Headers.RetryAfter;

                if (retryAfterHeader != null && retryAfterHeader.Delta.HasValue)
                {
                    var retryAfter = retryAfterHeader.Delta.Value;
                    throw new RetryAfterException(fcmRequest, "FCM Requested Backoff", DateTime.UtcNow + retryAfter);
                }
            }

            throw new FcmNotificationException(fcmRequest, "FCM HTTP Error: " + response.StatusCode, responseContentJson);
        }
    }
}
