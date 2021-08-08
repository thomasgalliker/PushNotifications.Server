using PushNotifications.Server.Google.Legacy;

namespace PushNotifications.Server.AspNetCore.Google.Legacy
{
    internal interface IFcmClientFactory
    {
        IFcmClient GetClient();

        bool TryGet(out IFcmClient fcmClient);
    }
}
