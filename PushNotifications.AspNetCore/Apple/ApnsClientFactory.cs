using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using PushNotifications.Apple;

namespace PushNotifications.AspNetCore
{
    public class ApnsClientFactory : IApnsClientFactory
    {
        private readonly IHttpClientFactory httpClientFactory;

        public ApnsClientFactory(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        public IApnsClient CreateUsingCert(X509Certificate2 cert, bool useSandbox, bool disableServerCertValidation = false)
        {
            var client = disableServerCertValidation
                ? this.CreateUsingCertWithNoServerCertValidation(cert)
                : ApnsClient.CreateUsingCert(cert);
            if (useSandbox)
                client.UseSandbox();
            return client;
        }

        public IApnsClient CreateUsingCert(string pathToCert, bool useSandbox = false, bool disableServerCertValidation = false)
        {
            var cert = new X509Certificate2(pathToCert);
            return this.CreateUsingCert(cert, useSandbox, disableServerCertValidation);
        }

        public IApnsClient CreateUsingJwt(ApnsJwtOptions options, bool useSandbox = false, bool disableServerCertValidation = false)
        {
            var httpClient = this.httpClientFactory.CreateClient(disableServerCertValidation ? "httpClient_PushNotifications_DisableCerverCertValidation" : "httpClient_PushNotifications");
            var client = new ApnsClient(httpClient, options);

            if (useSandbox)
            {
                client.UseSandbox();
            }

            return client;
        }

        private ApnsClient CreateUsingCertWithNoServerCertValidation(X509Certificate2 cert)
        {
            var handler = new HttpClientHandler
            {
                ClientCertificateOptions = ClientCertificateOption.Manual,
                ServerCertificateCustomValidationCallback = (a, b, c, d) => true
            };

            handler.ClientCertificates.Add(cert);
            var httpClient = new HttpClient(handler);
            var client = new ApnsClient(httpClient, cert);
            return client;
        }
    }
}
