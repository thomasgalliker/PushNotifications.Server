using System.Collections.Generic;
using System.Diagnostics;
using Newtonsoft.Json;

namespace PushNotifications.Server.Google
{
    [DebuggerDisplay("FcmResponse: IsSuccessful={this.IsSuccessful}")]
    public class FcmResponse : IPushResponse
    {
        [JsonIgnore]
        public string Token { get; internal set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        public bool IsSuccessful => this.Error == null;

        [JsonProperty("error")]
        public FcmError Error { get; set; }

        public IEnumerable<string> GetTokensWithRegistrationProblem()
        {
            if (this.Error is FcmError fcmError && 
                (fcmError.Status == FcmErrorCode.Unregistered || 
                 fcmError.Status == FcmErrorCode.NotFound))
            {
                yield return this.Token;
            }
        }
    }
}