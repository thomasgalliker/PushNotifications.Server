using System;

namespace PushNotifications.Abstractions
{
    public class NotificationException : Exception
    {
        public NotificationException (string message, IPushRequest notification) : base (message)
        {
            this.Notification = notification;
        }

        public NotificationException (string message, IPushRequest notification, Exception innerException)
            : base (message, innerException)
        {
            this.Notification = notification;
        }

        public IPushRequest Notification { get; set; }
    }
}
