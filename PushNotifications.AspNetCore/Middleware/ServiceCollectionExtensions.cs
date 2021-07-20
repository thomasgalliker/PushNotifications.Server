using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;

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

            services.AddSingleton<IApnsService, ApnsService>();
            services.AddSingleton<IFcmService, FcmService>();
            services.AddSingleton<IPushNotificationService, PushNotificationService>();

            return services;
        }
    }
}