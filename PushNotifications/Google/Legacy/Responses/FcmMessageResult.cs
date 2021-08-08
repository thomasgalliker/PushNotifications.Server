using Newtonsoft.Json;
using PushNotifications.Internals.JsonConverters;

namespace PushNotifications.Google.Legacy
{
    public class FcmMessageResult
    {
        /// <summary>
        /// The topic message ID when FCM has successfully received the request
        /// and will attempt to deliver to all subscribed devices.
        /// </summary>
        [JsonProperty("message_id", NullValueHandling = NullValueHandling.Ignore)]
        public string MessageId { get; set; }

        /// <summary>
        /// The registration ID (<seealso cref="FcmRequest.To"/> or <seealso cref="FcmRequest.RegistrationIds"/>)
        /// of the original request to which this result maps.
        /// </summary>
        [JsonIgnore]
        public string RegistrationId { get; internal set; }

        /// <summary>
        /// Error that occurred when processing the message.
        /// The possible values can be found in <seealso cref="FcmResponseStatus"/>.
        /// </summary>
        [JsonProperty("error", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(FcmResponseStatusJsonConverter))]
        public FcmResponseStatus Error { get; set; }
    }
}
