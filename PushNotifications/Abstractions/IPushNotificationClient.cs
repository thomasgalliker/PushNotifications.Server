using System.Threading;
using System.Threading.Tasks;

namespace PushNotifications
{
    public interface IPushNotificationClient
    {
        Task<IPushResponse> SendAsync(IPushRequest push, CancellationToken ct = default);
    }
}
