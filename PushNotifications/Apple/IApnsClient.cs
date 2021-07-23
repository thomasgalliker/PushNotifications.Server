using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace PushNotifications.Apple
{
    public interface IApnsClient
    {
        /// <exception cref="HttpRequestException">Exception occured during connection to an APNs service.</exception>
        /// <exception cref="ApnsCertificateExpiredException">APNs certificate used to connect to an APNs service is expired and needs to be renewed.</exception>
        Task<ApnsResponse> SendAsync(ApnsRequest request, CancellationToken ct = default);
    }
}