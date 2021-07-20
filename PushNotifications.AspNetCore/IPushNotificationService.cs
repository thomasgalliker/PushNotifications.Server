using System.Threading;
using System.Threading.Tasks;

namespace PushNotifications.AspNetCore
{
    /// <summary>
    /// Cross-platform push notification service.
    /// </summary>
    public interface IPushNotificationService
    {
        Task<PushResponse> SendAsync(PushRequest request, CancellationToken ct = default);
    }
}
