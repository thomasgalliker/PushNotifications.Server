using System;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PushNotifications.Google;

namespace PushNotifications.AspNetCore.Google
{
    internal class FcmClientFactory : IFcmClientFactory
    {
        private readonly IFcmClient client;

        public FcmClientFactory(ILogger<FcmClient> logger, IHttpClientFactory httpClientFactory, IOptions<PushNotificationsOptions> options)
        {
            var pushNotificationsOptions = options.Value;

            if (pushNotificationsOptions.FcmConfiguration is FcmConfiguration fcmConfiguration)
            {
                var httpClient = httpClientFactory.CreateClient(pushNotificationsOptions.DisableServerCertificateValidation
                    ? "httpClient_PushNotifications_DisableCerverCertValidation"
                    : "httpClient_PushNotifications");

                this.client = new FcmClient(new AspNetCoreLogger(logger), httpClient, fcmConfiguration);
            }
            else
            {
                throw new Exception("Configuration was not found");
            }
        }

        public IFcmClient GetClient()
        {
            return this.client;
        }
    }
}
