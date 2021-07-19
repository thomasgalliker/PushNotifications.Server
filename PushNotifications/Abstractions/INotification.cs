using System;

namespace PushNotifications.Abstractions
{
    public interface INotification
    {
        object Tag { get; set; }
    }
}
