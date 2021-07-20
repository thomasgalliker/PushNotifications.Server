using Newtonsoft.Json;
using PushNotifications.Abstractions;

namespace PushNotifications.Apple
{
    [JsonObject]
    public class ApnsResponse : IPushResponse
    {
        [JsonProperty("reason")]
        public ApnsResponseReason Reason { get; }

        [JsonProperty("success")]
        public bool IsSuccessful { get; }

        private ApnsResponse(ApnsResponseReason reason, bool isSuccessful)
        {
            this.Reason = reason;
            this.IsSuccessful = isSuccessful;
        }

        internal static ApnsResponse Successful()
        {
            return new ApnsResponse(null, true);
        }

        internal static ApnsResponse Error(ApnsResponseReason reason)
        {
            return new ApnsResponse(reason, false);
        }
    }
}