using PushNotifications.Server.Apple;

namespace PushNotifications.Server.AspNetCore.Apple
{
    internal interface IApnsClientFactory
    {
        IApnsClient GetClient();
    }
}
