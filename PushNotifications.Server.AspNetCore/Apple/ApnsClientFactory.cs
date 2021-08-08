using System;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PushNotifications.Server.Apple;
using PushNotifications.Server.AspNetCore.Logging;

namespace PushNotifications.Server.AspNetCore.Apple
{
    internal class ApnsClientFactory : IApnsClientFactory
    {
        private readonly IApnsClient client;

        public ApnsClientFactory(ILogger<ApnsClient> logger, IHttpClientFactory httpClientFactory, IOptions<PushNotificationsOptions> options)
        {
            var pushNotificationsOptions = options.Value;

            var httpClient = httpClientFactory.CreateClient(pushNotificationsOptions.DisableServerCertificateValidation
                ? "httpClient_PushNotifications_DisableCerverCertValidation"
                : "httpClient_PushNotifications");

            if (pushNotificationsOptions.ApnsJwtOptions is ApnsJwtOptions apnsJwtOptions)
            {
                this.client = new ApnsClient(new AspNetCoreLogger(logger), httpClient, apnsJwtOptions);
            }
            else
            {
                throw new ArgumentException("ApnsJwtOptions cannot be found", $"{nameof(PushNotificationsOptions)}.{nameof(pushNotificationsOptions.ApnsJwtOptions)}");
            }
        }

        public IApnsClient GetClient()
        {
            return this.client;
        }
    }
}
