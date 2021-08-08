using System.Collections.Generic;
using Newtonsoft.Json;

namespace PushNotifications.Google
{
    /// <summary>
    /// Source: https://firebase.google.com/docs/reference/fcm/rest/v1/projects.messages#ApnsConfig
    /// </summary>
    public class ApnsConfig
    {
        [JsonProperty("headers")]
        public IDictionary<string, string> Headers { get; set; }
        
        [JsonProperty("payload")]
        public ApnsConfigPayload Payload { get; set; }

        [JsonProperty("fcm_options")]
        public ApnsFcmOptions FcmOptions { get; set; }
    }
}
