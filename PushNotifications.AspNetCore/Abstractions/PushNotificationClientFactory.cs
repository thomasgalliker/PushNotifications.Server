using Microsoft.Extensions.Logging;
using PushNotifications.Apple;
using PushNotifications.Google;

namespace PushNotifications.AspNetCore
{
    internal class PushNotificationClientFactory : IPushNotificationClientFactory
    {
        private readonly IPushNotificationClient client;

        public PushNotificationClientFactory(ILogger<PushNotificationClient> logger, IFcmClient fcmClient, IApnsClient apnsClient)
        {
            this.client = new PushNotificationClient(new AspNetCoreLogger(logger), fcmClient, apnsClient);
        }

        public IPushNotificationClient GetClient()
        {
            return this.client;
        }
    }
}