using System.Collections.Generic;
using Newtonsoft.Json;

namespace PushNotifications.Server.Google
{
    /// <summary>
    /// Source: https://firebase.google.com/docs/reference/fcm/rest/v1/projects.messages#webpushconfig
    /// </summary>
    public class WebpushConfig
    {
        [JsonProperty("headers")]
        public IDictionary<string, string> Headers { get; set; }

        [JsonProperty("data")]
        public IDictionary<string, string> Data { get; set; }

        [JsonProperty("notification")]
        public WebpushNotification Notification { get; set; }

        [JsonProperty("fcm_options")]
        public WebpushFcmOptions FcmOptions { get; set; }
    }
}
