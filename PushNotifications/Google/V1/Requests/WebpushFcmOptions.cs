using Newtonsoft.Json;

namespace PushNotifications.Google
{
    /// <summary>
    /// Source: https://firebase.google.com/docs/reference/fcm/rest/v1/projects.messages#webpushfcmoptions
    /// </summary>
    public class WebpushFcmOptions
    {
        [JsonProperty("link")]
        public string Link { get; set; }

        [JsonProperty("analytics_label")]
        public string AnalyticsLabel { get; set; }
    }
}
