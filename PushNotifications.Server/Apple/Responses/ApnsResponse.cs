using System.Collections.Generic;
using System.Diagnostics;
using Newtonsoft.Json;

namespace PushNotifications.Server.Apple
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

        public ApnsResponse(string token)
        {
            this.Token = token;
            this.IsSuccessful = true;
        }
        
        public ApnsResponse(string token, ApnsResponseReason reason)
        {
            this.Token = token;
            this.Reason = reason;
            this.IsSuccessful = false;
        }

        public IEnumerable<string> GetTokensWithRegistrationProblem()
        {
            if (this.Reason == ApnsResponseReason.BadDeviceToken ||
                this.Reason == ApnsResponseReason.Unregistered || 
                this.Reason == ApnsResponseReason.MissingDeviceToken)
            {
                yield return this.Token;
            }
        }
    }
}