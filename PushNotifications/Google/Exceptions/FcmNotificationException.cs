using PushNotifications.Abstractions;

namespace PushNotifications.Google
{
    public class FcmNotificationException : NotificationException
    {
        internal FcmNotificationException(IPushRequest pushRequest, string message)
            : base(message, pushRequest)
        {
        }

        internal FcmNotificationException(IPushRequest pushRequest, string message, string description)
            : base(message, pushRequest)
        {
            this.Description = description;
        }

        public string Description { get; private set; }
    }
}

