using System.Diagnostics;
using Newtonsoft.Json;

namespace PushNotifications.Messages
{
    [JsonObject]
    [DebuggerDisplay("AppCenterPushMessage: {this.Content?.Name}")]
    public class AppCenterPushMessage
    {
        [JsonProperty("notification_content")]
        public AppCenterPushContent Content { get; set; }

        [JsonProperty("notification_target")]
        public AppCenterPushTarget Target { get; set; }
    }
}