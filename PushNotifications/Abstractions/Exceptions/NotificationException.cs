using System;

namespace PushNotifications.Abstractions
{
    public class NotificationException : Exception
    {
        public NotificationException (string message, INotification notification) : base (message)
        {
            this.Notification = notification;
        }

        public NotificationException (string message, INotification notification, Exception innerException)
            : base (message, innerException)
        {
            this.Notification = notification;
        }

        public INotification Notification { get; set; }
    }
}
