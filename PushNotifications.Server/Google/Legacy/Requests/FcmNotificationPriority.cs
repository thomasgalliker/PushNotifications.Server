using System.Runtime.Serialization;

namespace PushNotifications.Server.Google.Legacy
{
    public enum FcmNotificationPriority
    {
        [EnumMember(Value = "normal")]
        Normal = 5,
        [EnumMember(Value = "high")]
        High = 10
    }
}
