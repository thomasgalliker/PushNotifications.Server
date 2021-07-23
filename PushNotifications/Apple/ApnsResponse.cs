using System.Collections.Generic;
using System.Diagnostics;
using Newtonsoft.Json;

namespace PushNotifications.Apple
{
    [DebuggerDisplay("ApnsResponse: IsSuccessful={this.IsSuccessful}, Reason={this.Reason}")]
    [JsonObject]
    public class ApnsResponse : IPushResponse
    {
        [JsonIgnore]
        public string Token { get; }

        [JsonProperty("reason")]
        public ApnsResponseReason Reason { get; }

        [JsonProperty("success")]
        public bool IsSuccessful { get; }

        private ApnsResponse(string token, ApnsResponseReason reason, bool isSuccessful)
        {
            this.Token = token;
            this.Reason = reason;
            this.IsSuccessful = isSuccessful;
        }

        internal static ApnsResponse Successful(string token)
        {
            return new ApnsResponse(token, null, true);
        }

        internal static ApnsResponse Error(string token, ApnsResponseReason reason)
        {
            return new ApnsResponse(token, reason, false);
        }

        public IEnumerable<string> GetTokensWithRegistrationProblem()
        {
            if (this.Reason == ApnsResponseReason.BadDeviceToken || this.Reason == ApnsResponseReason.MissingDeviceToken)
            {
                yield return this.Token;
            }
        }
    }
}