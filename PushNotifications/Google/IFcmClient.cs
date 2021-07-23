using System.Threading;
using System.Threading.Tasks;

namespace PushNotifications.Google
{
    public interface IFcmClient
    {
        Task<FcmResponse> SendAsync(FcmRequest request, CancellationToken ct = default);
    }
}