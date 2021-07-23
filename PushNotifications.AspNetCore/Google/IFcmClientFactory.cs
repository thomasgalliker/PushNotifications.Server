using PushNotifications.Google;

namespace PushNotifications.AspNetCore.Google
{
    public interface IFcmClientFactory
    {
        IFcmClient GetClient();
    }
}
