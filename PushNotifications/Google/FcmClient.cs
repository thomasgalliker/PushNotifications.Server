using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PushNotifications.Abstractions;
using PushNotifications.Internals;

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

        public async Task<FcmResponse> SendAsync(FcmRequest request, CancellationToken ct = default)
        {

            // If 'To' was used instead of RegistrationIds, let's make RegistrationId's null
            // so we don't serialize an empty array for this property
            // otherwise, google will complain that we specified both instead
            if (request.RegistrationIds != null && request.RegistrationIds.Count <= 0 && !string.IsNullOrEmpty(request.To))
            {
                request.RegistrationIds = null;
            }

            var payload = JsonConvert.SerializeObject(request, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            var content = new JsonContent(payload);

            var response = await this.httpClient.PostAsync(this.configuration.FcmUrl, content);

            if (response.IsSuccessStatusCode)
            {
                await this.ProcessResponseOk(response, request).ConfigureAwait(false);
            }
            else
            {
                await this.ProcessResponseError(response, request).ConfigureAwait(false);
            }

            var result = new FcmResponse
            {
                ResponseCode = FcmResponseCode.Ok,
            };
            return result;
        }

        async Task ProcessResponseOk(HttpResponseMessage httpResponse, FcmRequest notification)
        {
            var multicastResult = new FcmMulticastResultException();

            var result = new FcmResponse
            {
                ResponseCode = FcmResponseCode.Ok,
                OriginalNotification = notification
            };

            var str = await httpResponse.Content.ReadAsStringAsync();
            var json = JObject.Parse(str);

            result.NumberOfCanonicalIds = json.Value<long>("canonical_ids");
            result.NumberOfFailures = json.Value<long>("failure");
            result.NumberOfSuccesses = json.Value<long>("success");

            var jsonResults = json["results"] as JArray ?? new JArray();

            foreach (var r in jsonResults)
            {
                var msgResult = new FcmMessageResult();

                msgResult.MessageId = r.Value<string>("message_id");
                msgResult.CanonicalRegistrationId = r.Value<string>("registration_id");
                msgResult.ResponseStatus = FcmResponseStatus.Ok;

                if (!string.IsNullOrEmpty(msgResult.CanonicalRegistrationId))
                    msgResult.ResponseStatus = FcmResponseStatus.CanonicalRegistrationId;
                else if (r["error"] != null)
                {
                    var err = r.Value<string>("error") ?? "";

                    msgResult.ResponseStatus = GetGcmResponseStatus(err);
                }

                result.Results.Add(msgResult);
            }

            int index = 0;

            //Loop through every result in the response
            // We will raise events for each individual result so that the consumer of the library
            // can deal with individual registrationid's for the notification
            foreach (var r in result.Results)
            {
                var singleResultNotification = FcmRequest.ForSingleResult(result, index);

                singleResultNotification.MessageId = r.MessageId;

                if (r.ResponseStatus == FcmResponseStatus.Ok)
                {
                    multicastResult.Succeeded.Add(singleResultNotification);
                }
                else if (r.ResponseStatus == FcmResponseStatus.CanonicalRegistrationId)
                { //Need to swap reg id's
                    //Swap Registrations Id's
                    var newRegistrationId = r.CanonicalRegistrationId;
                    var oldRegistrationId = string.Empty;

                    if (singleResultNotification.RegistrationIds != null && singleResultNotification.RegistrationIds.Count > 0)
                    {
                        oldRegistrationId = singleResultNotification.RegistrationIds[0];
                    }
                    else if (!string.IsNullOrEmpty(singleResultNotification.To))
                    {
                        oldRegistrationId = singleResultNotification.To;
                    }

                    multicastResult.Failed.Add(singleResultNotification,
                        new DeviceSubscriptionExpiredException(singleResultNotification)
                        {
                            OldSubscriptionId = oldRegistrationId,
                            NewSubscriptionId = newRegistrationId
                        });
                }
                else if (r.ResponseStatus == FcmResponseStatus.Unavailable)
                { // Unavailable
                    multicastResult.Failed.Add(singleResultNotification, new FcmNotificationException(singleResultNotification, "Unavailable Response Status"));
                }
                else if (r.ResponseStatus == FcmResponseStatus.NotRegistered)
                { //Bad registration Id
                    var oldRegistrationId = string.Empty;

                    if (singleResultNotification.RegistrationIds != null && singleResultNotification.RegistrationIds.Count > 0)
                    {
                        oldRegistrationId = singleResultNotification.RegistrationIds[0];
                    }
                    else if (!string.IsNullOrEmpty(singleResultNotification.To))
                    {
                        oldRegistrationId = singleResultNotification.To;
                    }

                    multicastResult.Failed.Add(singleResultNotification,
                                                new DeviceSubscriptionExpiredException(singleResultNotification)
                                                {
                                                    OldSubscriptionId = oldRegistrationId
                                                });
                }
                else
                {
                    multicastResult.Failed.Add(singleResultNotification, new FcmNotificationException(singleResultNotification, "Unknown Failure: " + r.ResponseStatus));
                }

                index++;
            }

            // If we only have 1 total result, it is not *multicast*, 
            if (multicastResult.Succeeded.Count + multicastResult.Failed.Count == 1)
            {
                // If not multicast, and succeeded, don't throw any errors!
                if (multicastResult.Succeeded.Count == 1)
                    return;
                // Otherwise, throw the one single failure we must have
                throw multicastResult.Failed.First().Value;
            }

            // If we get here, we must have had a multicast message
            // throw if we had any failures at all (otherwise all must be successful, so throw no error
            if (multicastResult.Failed.Count > 0)
            {
                throw multicastResult;
            }
        }

        async Task ProcessResponseError(HttpResponseMessage httpResponse, FcmRequest notification)
        {
            string responseBody = null;

            try
            {
                responseBody = await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
            }
            catch { }

            //401 bad auth token
            if (httpResponse.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new UnauthorizedAccessException("GCM Authorization Failed");
            }

            if (httpResponse.StatusCode == HttpStatusCode.BadRequest)
            {
                throw new FcmNotificationException(notification, "HTTP 400 Bad Request", responseBody);
            }

            if ((int)httpResponse.StatusCode >= 500 && (int)httpResponse.StatusCode < 600)
            {
                //First try grabbing the retry-after header and parsing it.
                var retryAfterHeader = httpResponse.Headers.RetryAfter;

                if (retryAfterHeader != null && retryAfterHeader.Delta.HasValue)
                {
                    var retryAfter = retryAfterHeader.Delta.Value;
                    throw new RetryAfterException(notification, "GCM Requested Backoff", DateTime.UtcNow + retryAfter);
                }
            }

            throw new FcmNotificationException(notification, "GCM HTTP Error: " + httpResponse.StatusCode, responseBody);
        }

        static FcmResponseStatus GetGcmResponseStatus(string str)
        {
            var enumType = typeof(FcmResponseStatus);

            foreach (var name in Enum.GetNames(enumType))
            {
                var enumMemberAttribute = ((EnumMemberAttribute[])enumType.GetField(name).GetCustomAttributes(typeof(EnumMemberAttribute), true)).Single();

                if (enumMemberAttribute.Value.Equals(str, StringComparison.InvariantCultureIgnoreCase))
                    return (FcmResponseStatus)Enum.Parse(enumType, name);
            }

            //Default
            return FcmResponseStatus.Error;
        }
    }
}
