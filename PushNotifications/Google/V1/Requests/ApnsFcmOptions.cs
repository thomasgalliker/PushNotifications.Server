using Newtonsoft.Json;

namespace PushNotifications.Google
{
    /// <summary>
    /// Source: https://firebase.google.com/docs/reference/fcm/rest/v1/projects.messages#ApnsFcmOptions
    /// </summary>
    public class ApnsFcmOptions
    {
        [JsonProperty("analytics_label")]
        public string AnalyticsLabel { get; set; }

        [JsonProperty("image")]
        public string Image { get; set; }
    }
}