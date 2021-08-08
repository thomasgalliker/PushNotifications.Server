using System;
using Newtonsoft.Json;
using PushNotifications.Server.Internals;
using PushNotifications.Server.Internals.JsonConverters;

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

        [JsonProperty("sticky")]
        public bool Sticky { get; set; }

        [JsonProperty("event_time")]
        public DateTime EventTime { get; set; }

        [JsonProperty("local_only")]
        public bool LocalOnly { get; set; }

        [JsonProperty("notification_priority")]
        [JsonConverter(typeof(NotificationPriorityJsonConverter))]
        public NotificationPriority NotificationPriority { get; set; }

        [JsonProperty("default_sound")]
        public bool DefaultSound { get; set; }

        [JsonProperty("default_vibrate_timings")]
        public bool DefaultVibrateTimings { get; set; }

        [JsonProperty("default_light_settings")]
        public bool DefaultLightSettings { get; set; }

        [JsonProperty("vibrate_timings")]
        [JsonConverter(typeof(DurationStringToTimeSpanArrayJsonConverter))]
        public TimeSpan[] VibrateTimings { get; set; }

        [JsonProperty("visibility")]
        [JsonConverter(typeof(VisibilityJsonConverter))]
        public Visibility Visibility { get; set; }

        [JsonProperty("notification_count")]
        public int NotificationCount { get; set; }

        [JsonProperty("light_settings")]
        public LightSettings LightSettings { get; set; }

        [JsonProperty("image")]
        public string Image { get; set; }
    }
}
