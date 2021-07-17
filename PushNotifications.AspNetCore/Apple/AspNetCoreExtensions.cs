using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;

namespace PushNotifications.AspNetCore
{
    public static class AspNetCoreExtensions
    {
        public static IServiceCollection AddPushNotifications(this IServiceCollection services, Action<ApnsServiceOptions> configureApns = null)
        {
            var optionsBuilder = services.AddOptions<ApnsServiceOptions>();
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
            services.AddSingleton<IApnsClientFactory, ApnsClientFactory>();
            services.AddSingleton<IApnsService, ApnsService>();
            return services;
        }
    }
}
