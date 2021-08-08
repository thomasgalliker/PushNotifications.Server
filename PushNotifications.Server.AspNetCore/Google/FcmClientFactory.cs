using System;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PushNotifications.Server.AspNetCore.Logging;
using PushNotifications.Server.Google;

namespace PushNotifications.Server.AspNetCore.Google
{
    internal class FcmClientFactory : IFcmClientFactory
    {
        private readonly ILogger<FcmClient> logger;
        private readonly IHttpClientFactory httpClientFactory;
        private readonly PushNotificationsOptions pushNotificationsOptions;
        private FcmClient client;

        public FcmClientFactory(ILogger<FcmClient> logger, IHttpClientFactory httpClientFactory, IOptions<PushNotificationsOptions> options)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            this.pushNotificationsOptions = options?.Value ?? throw new ArgumentNullException(nameof(options));
        }

        public IFcmClient GetClient()
        {
            if (this.TryGet(out var fcmClient))
            {
                return fcmClient;
            }

            throw new ArgumentException($"{nameof(this.pushNotificationsOptions.FcmOptions)} cannot be found", $"{nameof(PushNotificationsOptions)}.{nameof(this.pushNotificationsOptions.FcmOptions)}");
        }

        public bool TryGet(out IFcmClient fcmClient)
        {
            if (this.client != null)
            {
                fcmClient = this.client;
                return true;
            }

            var httpClient = this.httpClientFactory.CreateClient(this.pushNotificationsOptions.DisableServerCertificateValidation
                ? "httpClient_PushNotifications_DisableCerverCertValidation"
                : "httpClient_PushNotifications");

            if (this.pushNotificationsOptions.FcmOptions is FcmOptions fcmOptions)
            {
                this.client = new FcmClient(new AspNetCoreLogger(this.logger), httpClient, fcmOptions);
                fcmClient = this.client;
                return true;
            }
            else
            {
                fcmClient = null;
                return false;
            }
        }
    }
}
