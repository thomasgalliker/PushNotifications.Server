using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using PushNotifications.Apple;

namespace PushNotifications.AspNetCore
{
    public interface IApnsClientFactory
    {
        IApnsClient GetClient();
    }

    public class ApnsClientFactory : IApnsClientFactory
    {
        private readonly IApnsClient client;

        public ApnsClientFactory(IHttpClientFactory httpClientFactory, IOptions<PushNotificationsOptions> options)
        {
            var pushNotificationsOptions = options.Value;

            if (pushNotificationsOptions.ApnsJwtOptions is ApnsJwtOptions apnsJwtOptions)
            {
                var httpClient = httpClientFactory.CreateClient(pushNotificationsOptions.DisableServerCertificateValidation
                    ? "httpClient_PushNotifications_DisableCerverCertValidation"
                    : "httpClient_PushNotifications");

                this.client = new ApnsClient(httpClient, apnsJwtOptions);
            }
            else
            {
                throw new Exception("Configuration was not found");
            }
        }

        public IApnsClient GetClient()
        {
            return client;
        }
    }

    public class ApnsService : IApnsService
    {
        private readonly IApnsClient client;

        public ApnsService(IHttpClientFactory httpClientFactory, IOptions<PushNotificationsOptions> options)
        {
            var pushNotificationsOptions = options.Value;

            if (pushNotificationsOptions.ApnsJwtOptions is ApnsJwtOptions apnsJwtOptions)
            {
                var httpClient = httpClientFactory.CreateClient(pushNotificationsOptions.DisableServerCertificateValidation 
                    ? "httpClient_PushNotifications_DisableCerverCertValidation" 
                    : "httpClient_PushNotifications");

                this.client = new ApnsClient(httpClient, apnsJwtOptions);
            }
            else
            {
                throw new Exception("Configuration was not found");
            }
        }

        public Task<ApnsResponse> SendAsync(ApnsRequest push, CancellationToken ct = default)
        {
            return this.client.SendAsync(push);
        }
    }
}
