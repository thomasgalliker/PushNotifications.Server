using Newtonsoft.Json;

namespace PushNotifications.Server.Google
{
    /// <summary>
    /// Source: https://firebase.google.com/docs/reference/fcm/rest/v1/projects.messages#androidnotification
    /// </summary>
    public class AndroidNotification
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("body")]
        public string Body { get; set; }
        
        [JsonProperty("icon")]
        public string Icon { get; set; }

        [JsonProperty("color")]
        public string Color { get; set; }

        [JsonProperty("sound")]
        public string Sound { get; set; }

        [JsonProperty("tag")]
        public string Tag { get; set; }

        [JsonProperty("click_action")]
        public string ClickAction { get; set; }

        [JsonProperty("body_loc_key")]
        public string BodyLocKey { get; set; }

        [JsonProperty("body_loc_args")]
        public string[] BodyLocArgs { get; set; }

        [JsonProperty("title_loc_key")]
        public string TitleLocKey { get; set; }

        [JsonProperty("title_loc_args")]
        public string[] TitleLocArgs { get; set; }

        [JsonProperty("channel_id")]
        public string ChannelId { get; set; }
    }
}
