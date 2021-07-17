using System.Security.Cryptography.X509Certificates;
using PushNotifications.Apple;

namespace PushNotifications.AspNetCore
{
    public interface IApnsClientFactory
    {
        IApnsClient CreateUsingCert(X509Certificate2 cert, bool useSandbox = false, bool disableServerCertValidation = false);

        IApnsClient CreateUsingCert(string pathToCert, bool useSandbox = false, bool disableServerCertValidation = false);

        IApnsClient CreateUsingJwt(ApnsJwtOptions options, bool useSandbox = false, bool disableServerCertValidation = false);
    }
}
