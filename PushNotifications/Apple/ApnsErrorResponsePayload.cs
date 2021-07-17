using System;
using Newtonsoft.Json;

namespace PushNotifications.Apple
{
    public class ApnsErrorResponsePayload
    {
        private string reasonRaw;

        [JsonIgnore]
        public ApnsResponseReason Reason
        {
            get; private set;
        }

        [JsonProperty("reason")]
        public string ReasonRaw
        {
            get => this.reasonRaw;
            set
            {
                if (this.reasonRaw != value)
                {
                    this.reasonRaw = value;
                    this.Reason = Enum.TryParse<ApnsResponseReason>(this.ReasonRaw, out var enumValue) ? enumValue : ApnsResponseReason.Unknown;
                }
            }
        }

        [JsonConverter(typeof(UnixTimestampMillisecondsJsonConverter))] // timestamp is in milliseconds (https://openradar.appspot.com/24548417)
        public DateTimeOffset? Timestamp { get; set; }
    }
}