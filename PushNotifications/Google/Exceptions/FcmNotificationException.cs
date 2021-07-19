using PushNotifications.Abstractions;

namespace PushNotifications.Google
{
    public class FcmNotificationException : NotificationException
    {
        public FcmNotificationException (FcmRequest notification, string msg) : base (msg, notification)
        {
            this.Notification = notification;
        }

        public FcmNotificationException (FcmRequest notification, string msg, string description) : base (msg, notification)
        {
            this.Notification = notification;
            this.Description = description;
        }

        public new FcmRequest Notification { get; private set; }

        public string Description { get; private set; }
    }
}

