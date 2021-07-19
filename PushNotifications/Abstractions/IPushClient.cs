using System.Threading;
using System.Threading.Tasks;

namespace PushNotifications.Abstractions
{
    public interface IPushClient
    {
        Task<IPushResponse> SendAsync(IPushRequest push, CancellationToken ct = default);
    }
}
