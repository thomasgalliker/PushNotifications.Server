using System.Threading;
using System.Threading.Tasks;

namespace PushNotifications
{
    public interface IPushNotificationClient
    {
        Task<PushResponse> SendAsync(PushRequest push, CancellationToken ct = default);
    }
}
