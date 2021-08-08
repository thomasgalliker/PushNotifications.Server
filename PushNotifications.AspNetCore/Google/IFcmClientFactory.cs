using PushNotifications.Google;

namespace PushNotifications.AspNetCore.Google
{
    internal interface IFcmClientFactory
    {
        IFcmClient GetClient();

        bool TryGet(out IFcmClient fcmClient);
    }
}
