using System.Diagnostics;
using Newtonsoft.Json;

namespace PushNotifications.Google
{
    [DebuggerDisplay("FcmError: Code={this.Code}, Status={this.Status}")]
    public class FcmError
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("status")]
        public FcmErrorCode Status { get; set; }
    }
}