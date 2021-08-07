
using Newtonsoft.Json;

namespace PushNotifications.Google
{
    public class WebpushNotification
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("body")]
        public string Body { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }
    }
}