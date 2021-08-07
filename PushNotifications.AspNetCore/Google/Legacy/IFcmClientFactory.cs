using PushNotifications.Google.Legacy;

namespace PushNotifications.AspNetCore.Google.Legacy
{
    internal interface IFcmClientFactory
    {
        IFcmClient GetClient();

        bool TryGet(out IFcmClient fcmClient);
    }
}
