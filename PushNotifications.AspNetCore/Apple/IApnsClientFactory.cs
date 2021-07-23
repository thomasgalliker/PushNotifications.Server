using PushNotifications.Apple;

namespace PushNotifications.AspNetCore.Apple
{
    public interface IApnsClientFactory
    {
        IApnsClient GetClient();
    }
}
