using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PushNotifications.Apple;
using PushNotifications.Google;
using PushNotifications.Logging;

namespace PushNotifications
{
    /// <summary>
    /// Cross-platform implementation of a push notification client.
    /// </summary>
    public class PushNotificationClient : IPushNotificationClient
    {
        private readonly IFcmClient fcmClient;
        private readonly IApnsClient apnsClient;

        public PushNotificationClient(IFcmClient fcmClient, IApnsClient apnsClient)
        {
            this.fcmClient = fcmClient;
            this.apnsClient = apnsClient;
        }

        public async Task<PushResponse> SendAsync(PushRequest pushRequest, CancellationToken ct = default)
        {
            var apnsResponses = new List<ApnsResponse>();
            var fcmResponses = new List<FcmResponse>();

            var iOSPushDevices = pushRequest.Devices.Where(d => d.Platform == RuntimePlatform.iOS).ToList();
            var androidPushDevices = pushRequest.Devices.Where(d => d.Platform == RuntimePlatform.Android).ToList();
            Logger.Info($"SendAsync sends PushRequest to {iOSPushDevices.Count + androidPushDevices.Count} devices ({iOSPushDevices.Count} iOS, {androidPushDevices.Count} Android)");

            if (iOSPushDevices.Any())
            {
                foreach (var pushDevice in iOSPushDevices)
                {
                    var apnsRequest = new ApnsRequest(ApplePushType.Alert)
                        .AddToken(pushDevice.DeviceToken)
                        .AddAlert(pushRequest.Content.Title, pushRequest.Content.Body);

                    foreach (var item in pushRequest.Content.CustomData)
                    {
                        apnsRequest.AddCustomProperty(item.Key, item.Value);
                    }

                    var apnsResponse = await this.apnsClient.SendAsync(apnsRequest, ct);
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
                        Title = pushRequest.Content.Title,
                        Body = pushRequest.Content.Body,
                    },
                    Data = pushRequest.Content.CustomData
                };

                var fcmResponse = await this.fcmClient.SendAsync(fcmRequest, ct);
                fcmResponses.Add(fcmResponse);
            }

            var apnsPushResults = apnsResponses.Select(r => new PushResult { OriginalResponse = r, DeviceToken = r.Token, IsSuccessful = r.IsSuccessful });

            var fcmPushResults = fcmResponses.SelectMany(r => r.Results.Select(x => new PushResult { OriginalResponse = r, DeviceToken = x.RegistrationId, IsSuccessful = x.Error == null }));

            return new PushResponse(apnsPushResults.Union(fcmPushResults).ToList());
        }
    }
}
