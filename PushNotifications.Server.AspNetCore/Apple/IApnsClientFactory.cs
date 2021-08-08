using PushNotifications.Server.Apple;

namespace PushNotifications.Server.Server.AspNetCore.Apple
{
    internal interface IApnsClientFactory
    {
        IApnsClient GetClient();
    }
}
