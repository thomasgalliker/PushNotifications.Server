using System.Collections.Generic;
using Newtonsoft.Json;

namespace PushNotifications.Server.Google
{
    /// <summary>
    /// Source: https://firebase.google.com/docs/reference/fcm/rest/v1/projects.messages#Message
    /// </summary>
    public class Message
    {
        [JsonProperty("data")]
        public IDictionary<string, string> Data { get; set; }

        [JsonProperty("notification")]
        public Notification Notification { get; set; }

        [JsonProperty("android")]
        public AndroidConfig AndroidConfig { get; set; }

        [JsonProperty("webpush")]
        public WebpushConfig WebpushConfig { get; set; }

        [JsonProperty("apns")]
        public ApnsConfig ApnsConfig { get; set; }

        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("topic")]
        public string Topic { get; set; }

        [JsonProperty("condition")]
        public string Condition { get; set; }
    }
}