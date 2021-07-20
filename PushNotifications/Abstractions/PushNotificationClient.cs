using System.Threading;
using System.Threading.Tasks;
using PushNotifications.Apple;
using PushNotifications.Google;

namespace PushNotifications
{
    public class PushNotificationClient : IPushNotificationClient
    {
        public Task<IPushResponse> SendAsync(IPushRequest push, CancellationToken ct = default)
        {
            if (push is PushRequest)
            {
                // TODO
            }

            if (push is ApnsRequest)
            {
                // TODO
            }

            if (push is FcmRequest)
            {
                // TODO
            }

            return null;
        }
    }
}
