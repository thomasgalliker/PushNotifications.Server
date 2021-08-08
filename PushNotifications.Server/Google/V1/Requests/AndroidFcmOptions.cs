using Newtonsoft.Json;

namespace PushNotifications.Server.Google
{
    /// <summary>
    /// Source: https://firebase.google.com/docs/reference/fcm/rest/v1/projects.messages#androidfcmoptions
    /// </summary>
    public class AndroidFcmOptions
    {
        [JsonProperty("analytics_label")]
        public string AnalyticsLabel { get; set; }
    }
}