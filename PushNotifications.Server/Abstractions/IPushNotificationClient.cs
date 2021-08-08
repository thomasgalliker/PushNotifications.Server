using System.Threading;
using System.Threading.Tasks;

namespace PushNotifications.Server
{
    /// <summary>
    /// Cross-platform abstraction for a push notification client.
    /// </summary>
    public interface IPushNotificationClient
    {
        Task<PushResponse> SendAsync(PushRequest pushRequest, CancellationToken ct = default);
    }
}
