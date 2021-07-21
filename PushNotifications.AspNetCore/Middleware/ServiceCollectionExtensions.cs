using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using PushNotifications.Apple;
using PushNotifications.AspNetCore.Apple;
using PushNotifications.AspNetCore.Google;
using PushNotifications.Google;

namespace PushNotifications.AspNetCore
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPushNotifications(this IServiceCollection services, Action<PushNotificationsOptions> configureApns = null)
        {
            var optionsBuilder = services.AddOptions<PushNotificationsOptions>();
            if (configureApns != null)
            {
                optionsBuilder.Configure(configureApns);
            }

            services.AddHttpClient("httpClient_PushNotifications");
            services.AddHttpClient("httpClient_PushNotifications_DisableCerverCertValidation")
                .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler()
                {
                    ServerCertificateCustomValidationCallback = (m, x, c, s) => true
                });

            services.AddSingleton<IFcmClient>(s => s.GetService<IFcmClientFactory>().GetClient());
            services.AddSingleton<IApnsClient>(s => s.GetService<IApnsClientFactory>().GetClient());
            services.AddSingleton<IPushNotificationClient, PushNotificationClient>();
            services.AddSingleton<IApnsClientFactory, ApnsClientFactory>();
            services.AddSingleton<IFcmClientFactory, FcmClientFactory>();

            return services;
        }
    }
}