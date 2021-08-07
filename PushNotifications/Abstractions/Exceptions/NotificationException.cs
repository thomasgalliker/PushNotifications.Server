using System;

namespace PushNotifications.Abstractions
{
    public class NotificationException : Exception
    {
        internal NotificationException (string message, IPushRequest notification) : base (message)
        {
            this.PushRequest = notification;
        }

        internal NotificationException (string message, IPushRequest notification, Exception innerException)
            : base (message, innerException)
        {
            this.PushRequest = notification;
        }

        public IPushRequest PushRequest { get; }
    }
}
