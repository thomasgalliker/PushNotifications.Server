using System;

namespace PushNotifications.Abstractions
{
    public class NotificationException : Exception
    {
        internal NotificationException (string message, IPushRequest notification) : base (message)
        {
            this.Notification = notification;
        }

        internal NotificationException (string message, IPushRequest notification, Exception innerException)
            : base (message, innerException)
        {
            this.Notification = notification;
        }

        public IPushRequest Notification { get; }
    }
}
