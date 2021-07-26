using PushNotifications.Apple;

namespace PushNotifications.AspNetCore.Apple
{
    internal interface IApnsClientFactory
    {
        IApnsClient GetClient();
    }
}
