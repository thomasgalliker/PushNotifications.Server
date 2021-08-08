using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using PushNotifications.Apple;
using PushNotifications.AspNetCore.Apple;

using FcmClientFactory = PushNotifications.AspNetCore.Google.FcmClientFactory;
using FcmLegacyClientFactory = PushNotifications.AspNetCore.Google.Legacy.FcmClientFactory;
using IFcmClient = PushNotifications.Google.IFcmClient;
using IFcmClientFactory = PushNotifications.AspNetCore.Google.IFcmClientFactory;
using IFcmLegacyClient = PushNotifications.Google.Legacy.IFcmClient;
using IFcmLegacyClientFactory = PushNotifications.AspNetCore.Google.Legacy.IFcmClientFactory;

namespace PushNotifications.AspNetCore
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPushNotifications(this IServiceCollection services, Action<PushNotificationsOptions> options = null)
        {
            var optionsBuilder = services.AddOptions<PushNotificationsOptions>();
            if (options != null)
            {
                optionsBuilder.Configure(options);
            }

            services.AddHttpClient("httpClient_PushNotifications");
            services.AddHttpClient("httpClient_PushNotifications_DisableCerverCertValidation")
                .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler()
                {
                    ServerCertificateCustomValidationCallback = (m, x, c, s) => true
                });

            services.AddSingleton<IApnsClientFactory, ApnsClientFactory>();
            services.AddSingleton<IFcmClientFactory, FcmClientFactory>();
            services.AddSingleton<IFcmLegacyClientFactory, FcmLegacyClientFactory>();
            services.AddSingleton<IPushNotificationClientFactory, PushNotificationClientFactory>();

            services.AddSingleton<IFcmClient>(s => s.GetService<IFcmClientFactory>().GetClient());
            services.AddSingleton<IFcmLegacyClient>(s => s.GetService<IFcmLegacyClientFactory>().GetClient());
            services.AddSingleton<IApnsClient>(s => s.GetService<IApnsClientFactory>().GetClient());
            services.AddSingleton<IPushNotificationClient>(s => s.GetService<IPushNotificationClientFactory>().GetClient());
            return services;
        }
    }
}