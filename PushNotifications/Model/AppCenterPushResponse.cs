using System.Diagnostics;
using Newtonsoft.Json;

namespace PushNotifications.Messages
{
    [JsonObject]
    public class AppCenterPushResponse
    {
        [JsonConverter(typeof(RuntimePlatformJsonConverter))]
        public RuntimePlatform RuntimePlatform { get; set; }
    }

    [DebuggerDisplay("AppCenterPushSuccess: {this.NotificationId}")]
    public class AppCenterPushSuccess : AppCenterPushResponse
    {
        [JsonProperty("notification_id")]
        public string NotificationId { get; set; }
    }

    [DebuggerDisplay("AppCenterPushError: Message={this.ErrorMessage}, Code={this.ErrorCode}")]
    public class AppCenterPushError : AppCenterPushResponse
    {
        [JsonProperty("code")]
        public string ErrorCode { get; set; }

        [JsonProperty("message")]
        public string ErrorMessage { get; set; }
    }
}