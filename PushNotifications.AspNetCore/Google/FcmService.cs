using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using PushNotifications.Google;

namespace PushNotifications.AspNetCore
{
    public class FcmService : IFcmService
    {
        private readonly IFcmClient client;

        public FcmService(IHttpClientFactory httpClientFactory, IOptions<PushNotificationsOptions> options)
        {
            var pushNotificationsOptions = options.Value;

            if (pushNotificationsOptions.FcmConfiguration is FcmConfiguration fcmConfiguration)
            {
                var httpClient = httpClientFactory.CreateClient(pushNotificationsOptions.DisableServerCertificateValidation 
                    ? "httpClient_PushNotifications_DisableCerverCertValidation" 
                    : "httpClient_PushNotifications");

                this.client = new FcmClient(httpClient, fcmConfiguration);
            }
            else
            {
                throw new Exception("Configuration was not found");
            }
        }

        public Task<FcmResponse> SendAsync(FcmRequest push, CancellationToken ct = default)
        {
            return this.client.SendAsync(push);
        }
    }
}
