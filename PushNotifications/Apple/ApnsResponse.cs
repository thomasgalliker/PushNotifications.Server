using Newtonsoft.Json;
using PushNotifications.Abstractions;

namespace PushNotifications.Apple
{
    public class ApnsResponse : PushResponse
    {
        public ApnsResponseReason Reason { get; }

        public string ReasonString { get; }

        public bool IsSuccessful { get; }

        [JsonConstructor]
        ApnsResponse(ApnsResponseReason reason, string reasonString, bool isSuccessful)
        {
            this.Reason = reason;
            this.ReasonString = reasonString;
            this.IsSuccessful = isSuccessful;
        }

        public static readonly ApnsResponse Successful = new ApnsResponse(ApnsResponseReason.Success, null, true);

        public static ApnsResponse Error(ApnsResponseReason reason, string reasonString) => new ApnsResponse(reason, reasonString, false);
    }
}