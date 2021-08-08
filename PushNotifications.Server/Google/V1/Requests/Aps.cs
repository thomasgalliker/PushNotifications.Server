
using System.Collections.Generic;
using Newtonsoft.Json;
using PushNotifications.Server.Internals;

namespace PushNotifications.Server.Google
{
    /// <summary>
    /// Source: https://developer.apple.com/documentation/usernotifications/setting_up_a_remote_notification_server/generating_a_remote_notification
    /// </summary>
    public class Aps
    {
        [JsonProperty("alert")]
        public ApsAlert Alert { get; set; }

        [JsonProperty("badge")]
        public int? Badge { get; set; }

        [JsonProperty("sound")]
        public string Sound { get; set; }

        [JsonProperty("content-available")]
        [JsonConverter(typeof(BoolToIntJsonConverter))]
        public bool ContentAvailable { get; set; }

        [JsonProperty("mutable-content")]
        [JsonConverter(typeof(BoolToIntJsonConverter))]
        public bool MutableContent { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }

        [JsonProperty("thread-id")]
        public string ThreadId { get; set; }

        [JsonExtensionData]
        public IDictionary<string, object> CustomData { get; set; }
    }
}
