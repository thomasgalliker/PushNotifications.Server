using System.Diagnostics;
using Newtonsoft.Json;

namespace PushNotifications.Messages
{
    [DebuggerDisplay("AppCenterPushTarget: {this.Type}")]
    public abstract class AppCenterPushTarget
    {
        [JsonProperty("type")]
        public abstract string Type { get; }
    }
}