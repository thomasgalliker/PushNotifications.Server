using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace PushNotifications.AspNetCore
{
    public class PushNotificationService : IPushNotificationService
    {
        private readonly ILogger<PushNotificationService> logger;
        private readonly IPushNotificationClient pushNotificationClient;

        public PushNotificationService(
            ILogger<PushNotificationService> logger,
            IPushNotificationClient pushNotificationClient)
        {
            this.logger = logger;
            this.pushNotificationClient = pushNotificationClient;
        }

        public async Task<IPushResponse> SendAsync(IPushRequest pushRequest, CancellationToken ct = default)
        {
            var pushResponse = await this.pushNotificationClient.SendAsync(pushRequest, ct);
            return pushResponse;
        }
    }
}
