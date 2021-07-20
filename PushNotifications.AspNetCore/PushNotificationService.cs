using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PushNotifications.Apple;
using PushNotifications.Google;

namespace PushNotifications.AspNetCore
{
    public class PushNotificationService : IPushNotificationService
    {
        private readonly ILogger<PushNotificationService> logger;
        private readonly IFcmService fcmService;
        private readonly IApnsService apnsService;

        public PushNotificationService(
            ILogger<PushNotificationService> logger,
            IFcmService fcmService,
            IApnsService apnsService)
        {
            this.logger = logger;
            this.fcmService = fcmService;
            this.apnsService = apnsService;
        }

        public async Task<PushResponse> SendAsync(PushRequest request, CancellationToken ct = default)
        {
            var apnsResponses = new List<ApnsResponse>();
            var fcmResponses = new List<FcmResponse>();

            var iOSPushDevices = request.Devices.Where(d => d.Platform == RuntimePlatform.iOS).ToList();
            var androidPushDevices = request.Devices.Where(d => d.Platform == RuntimePlatform.Android).ToList();
            this.logger.LogInformation($"SendAsync to {iOSPushDevices.Count + androidPushDevices.Count} devices ({iOSPushDevices.Count} iOS, {androidPushDevices.Count} Android)");

            if (iOSPushDevices.Any())
            {
                foreach (var pushDevice in iOSPushDevices)
                {
                    var apnsRequest = new ApnsRequest(ApplePushType.Alert)
                        .AddToken(pushDevice.DeviceToken)
                        .AddAlert(request.Content.Title, request.Content.Body);

                    foreach (var item in request.Content.CustomData)
                    {
                        apnsRequest.AddCustomProperty(item.Key, item.Value);
                    }

                    var apnsResponse = await apnsService.SendAsync(apnsRequest, ct);
                    apnsResponses.Add(apnsResponse);
                }
            }

            if (androidPushDevices.Any())
            {
                var fcmRequest = new FcmRequest()
                {
                    RegistrationIds = androidPushDevices.Select(d => d.DeviceToken).ToList(),
                    Notification = new FcmNotification
                    {
                        Title = request.Content.Title,
                        Body = request.Content.Body,
                    },
                    Data = request.Content.CustomData
                };

                var fcmResponse = await fcmService.SendAsync(fcmRequest, ct);
                fcmResponses.Add(fcmResponse);
            }

            // TODO: Implement cross-platform response format: Map Android/iOS error codes to a new error code
            return new PushResponse
            {
                IsSuccessful = !fcmResponses.Any(r => r.IsSuccessful == false) && !apnsResponses.Any(r => r.IsSuccessful == false)
            };
        }
    }
}
