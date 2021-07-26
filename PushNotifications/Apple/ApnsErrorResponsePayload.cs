using System;
using Newtonsoft.Json;
using PushNotifications.Internals;

namespace PushNotifications.Apple
{
    /// <summary>
    /// Source: https://developer.apple.com/documentation/usernotifications/setting_up_a_remote_notification_server/handling_notification_responses_from_apns
    /// </summary>
    public class ApnsErrorResponsePayload
    {
        /// <summary>
        /// The error code indicating the reason for the failure.
        /// For a list of possible values, see <seealso cref="ApnsResponseReason"/>.
        /// </summary>
        [JsonProperty("reason", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(ApnsResponseReasonJsonConverter))]
        public ApnsResponseReason Reason { get; set; }

        /// <summary>
        /// The time at which APNs confirmed the token was no longer valid for the topic.
        /// This key is included only when the error in the :status field is 410.
        /// </summary>
        [JsonConverter(typeof(UnixTimestampMillisecondsJsonConverter))]
        public DateTimeOffset? Timestamp { get; set; }
    }
}