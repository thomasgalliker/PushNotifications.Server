using System;

namespace PushNotifications.Abstractions
{
    public class RetryAfterException : NotificationException
    {
        public RetryAfterException (IPushRequest notification, string message, DateTime retryAfterUtc) : base (message, notification)
        {
            this.RetryAfterUtc = retryAfterUtc;
        }

        public DateTime RetryAfterUtc { get; set; }
    }
}
