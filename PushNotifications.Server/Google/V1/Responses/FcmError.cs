using System.Diagnostics;
using Newtonsoft.Json;
using PushNotifications.Server.Internals.JsonConverters;

namespace PushNotifications.Server.Google
{
    [DebuggerDisplay("FcmError: Code={this.Code}, Status={this.Status}")]
    public class FcmError
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("status")]
        [JsonConverter(typeof(FcmErrorCodeJsonConverter))]
        public FcmErrorCode Status { get; set; }
    }
}