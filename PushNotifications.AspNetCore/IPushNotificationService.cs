using System.Threading;
using System.Threading.Tasks;

namespace PushNotifications.AspNetCore
{
    /// <summary>
    /// Cross-platform push notification service.
    /// </summary>
    public interface IPushNotificationService
    {
        Task<IPushResponse> SendAsync(IPushRequest request, CancellationToken ct = default);
    }
}
