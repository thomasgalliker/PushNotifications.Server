using System.Runtime.Serialization;

namespace PushNotifications.Google
{
    public enum FcmNotificationPriority
    {
        [EnumMember (Value="normal")]
        Normal = 5,
        [EnumMember (Value="high")]
        High = 10
    }
}
