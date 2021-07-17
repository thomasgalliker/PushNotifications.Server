using Newtonsoft.Json;

namespace PushNotifications.Messages
{
    public enum NotificationState
    {
        Unknown,
        Processing,
        Enqueued,
        Completed,
        Cancelled
    }
}