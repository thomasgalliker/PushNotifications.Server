using System;
using Microsoft.Extensions.Logging;
using PushNotifications.Server.Apple;
using PushNotifications.Server.AspNetCore.Apple;
using PushNotifications.Server.AspNetCore.Logging;
using FcmClient = PushNotifications.Server.Google.FcmClient;
using FcmLegacyClient = PushNotifications.Server.Google.Legacy.FcmClient;
using IFcmClient = PushNotifications.Server.Google.IFcmClient;
using IFcmClientFactory = PushNotifications.Server.AspNetCore.Google.IFcmClientFactory;
using IFcmLegacyClient = PushNotifications.Server.Google.Legacy.IFcmClient;
using IFcmLegacyClientFactory = PushNotifications.Server.AspNetCore.Google.Legacy.IFcmClientFactory;

namespace PushNotifications.Server.AspNetCore
{
    internal class PushNotificationClientFactory : IPushNotificationClientFactory
    {
        private readonly IPushNotificationClient pushNotificationClient;

        public PushNotificationClientFactory(
            ILogger<PushNotificationClient> logger,
            IFcmClientFactory fcmClientFactory,
            IFcmLegacyClientFactory fcmLegacyClientFactory,
            IApnsClientFactory apnsClientFactory,
            IApnsClient apnsClient)
        {
            if (fcmClientFactory.TryGet(out var fcmClient))
            {
                this.pushNotificationClient = new PushNotificationClient(new AspNetCoreLogger(logger), fcmClient, apnsClient);
            }
            else if (fcmLegacyClientFactory.TryGet(out var fcmLegacyClient))
            {
                this.pushNotificationClient = new PushNotificationClient(new AspNetCoreLogger(logger), fcmLegacyClient, apnsClient);
            }
            else
            {
                throw new ArgumentException(
                    $"Unable to resolve {nameof(IFcmClient)}. Check if there is a valid configuration for either {Environment.NewLine}" +
                    $"{nameof(PushNotificationsOptions)}.{nameof(PushNotificationsOptions.FcmOptions)}, if you want to use {typeof(IFcmClient).FullName} ({FcmClient.ApiName}) or " +
                    $"{nameof(PushNotificationsOptions)}.{nameof(PushNotificationsOptions.FcmLegacyOptions)}, if you want to use {typeof(IFcmLegacyClient).FullName} ({FcmLegacyClient.ApiName})");
            }
        }

        public IPushNotificationClient GetClient()
        {
            return this.pushNotificationClient;
        }
    }
}