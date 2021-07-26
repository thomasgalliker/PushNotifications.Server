using System;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PushNotifications.Apple;
using PushNotifications.AspNetCore.Logging;

namespace PushNotifications.AspNetCore.Apple
{
    internal class ApnsClientFactory : IApnsClientFactory
    {
        private readonly IApnsClient client;

        public ApnsClientFactory(ILogger<ApnsClient> logger, IHttpClientFactory httpClientFactory, IOptions<PushNotificationsOptions> options)
        {
            var pushNotificationsOptions = options.Value;

            if (pushNotificationsOptions.ApnsJwtOptions is ApnsJwtOptions apnsJwtOptions)
            {
                var httpClient = httpClientFactory.CreateClient(pushNotificationsOptions.DisableServerCertificateValidation
                    ? "httpClient_PushNotifications_DisableCerverCertValidation"
                    : "httpClient_PushNotifications");

                this.client = new ApnsClient(new AspNetCoreLogger(logger), httpClient, apnsJwtOptions);
            }
            else
            {
                throw new Exception("Configuration was not found");
            }
        }

        public IApnsClient GetClient()
        {
            return this.client;
        }
    }
}
