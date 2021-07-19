using System;

namespace PushNotifications.Abstractions
{
    public class DeviceSubscriptionExpiredException : NotificationException
    {
        public DeviceSubscriptionExpiredException(INotification notification) : base ("Device Subscription has Expired", notification)
        {
            this.ExpiredAt = DateTime.UtcNow;
        }

        public string OldSubscriptionId { get; set; }
        public string NewSubscriptionId { get; set; }
        public DateTime ExpiredAt { get; set; }
    }
}
