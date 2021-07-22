using System;
using PushNotifications.Abstractions;

namespace PushNotifications.Google
{
    public class RetryAfterException : NotificationException
    {
        internal RetryAfterException (IPushRequest notification, string message, DateTime retryAfterUtc) : base (message, notification)
        {
            this.RetryAfterUtc = retryAfterUtc;
        }

        public DateTime RetryAfterUtc { get; set; }
    }
}
