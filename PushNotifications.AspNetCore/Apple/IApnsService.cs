using System.Threading.Tasks;
using PushNotifications.Apple;

namespace PushNotifications.AspNetCore
{
    public interface IApnsService
    {
        Task<ApnsResponse> SendAsync(ApplePush push);
    }
}
