using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PushNotifications.Apple;
using PushNotifications.Google;
using PushNotifications.Google.Legacy;
using PushNotifications.Logging;
using FcmLegacyRequest = PushNotifications.Google.Legacy.FcmRequest;
using FcmLegacyResponse = PushNotifications.Google.Legacy.FcmResponse;
using FcmRequest = PushNotifications.Google.FcmRequest;
using FcmResponse = PushNotifications.Google.FcmResponse;
using IFcmClient = PushNotifications.Google.IFcmClient;
using IFcmLegacyClient = PushNotifications.Google.Legacy.IFcmClient;

namespace PushNotifications
{
    /// <summary>
    /// Cross-platform implementation of a push notification client.
    /// </summary>
    public class PushNotificationClient : IPushNotificationClient
    {
        private readonly ILogger logger;
        private readonly IFcmClient fcmClient;
        private readonly IFcmLegacyClient fcmLegacyClient;
        private readonly IApnsClient apnsClient;

        public PushNotificationClient(IFcmClient fcmClient, IApnsClient apnsClient)
            : this(Logger.Current, fcmClient, apnsClient)
        {
        }

        public PushNotificationClient(ILogger logger, IFcmClient fcmClient, IApnsClient apnsClient)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.fcmClient = fcmClient ?? throw new ArgumentNullException(nameof(fcmClient));
            this.apnsClient = apnsClient ?? throw new ArgumentNullException(nameof(apnsClient));
        }

        public PushNotificationClient(IFcmLegacyClient fcmClient, IApnsClient apnsClient)
            : this(Logger.Current, fcmClient, apnsClient)
        {
        }

        public PushNotificationClient(ILogger logger, IFcmLegacyClient fcmLegacyClient, IApnsClient apnsClient)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.fcmLegacyClient = fcmLegacyClient ?? throw new ArgumentNullException(nameof(fcmLegacyClient));
            this.apnsClient = apnsClient ?? throw new ArgumentNullException(nameof(apnsClient));
        }

        public async Task<PushResponse> SendAsync(PushRequest pushRequest, CancellationToken ct = default)
        {
            var apnsResponses = new List<ApnsResponse>();
            var fcmResponses = new List<FcmResponse>();
            var fcmLegacyResponses = new List<FcmLegacyResponse>();

            var apnsPushDevices = pushRequest.Devices.Where(d => d.Platform == RuntimePlatform.iOS).ToList();
            var fcmPushDevices = pushRequest.Devices.Where(d => d.Platform == RuntimePlatform.Android).ToList();

            var platFormCounts = pushRequest.Devices
                .GroupBy(d => d.Platform)
                .Select(g => new { Key = g.Key.ToString(), Count = g.Count() })
                .Where(g => g.Count > 0)
                .OrderBy(g => g.Key)
                .Select(g => $"{g.Count} {g.Key}");

            var platformCountSummary = string.Join(", ", platFormCounts);
            this.logger.Log(LogLevel.Info, $"SendAsync sends PushRequest to {apnsPushDevices.Count + fcmPushDevices.Count} devices ({platformCountSummary})");

            // Handle APNS push notifications
            if (apnsPushDevices.Any())
            {
                foreach (var pushDevice in apnsPushDevices)
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

            // Handle FCM push notifications
            if (fcmPushDevices.Any())
            {
                if (this.fcmClient != null)
                {
                    foreach (var pushDevice in fcmPushDevices)
                    {
                        var fcmRequest = new FcmRequest()
                        {
                            Message = new Message
                            {
                                Token = pushDevice.DeviceToken,
                                Notification = new Notification
                                {
                                    Title = pushRequest.Content.Title,
                                    Body = pushRequest.Content.Body,
                                },
                                Data = pushRequest.Content.CustomData
                            },
                            ValidateOnly = false,
                        };

                        var fcmResponse = await this.fcmClient.SendAsync(fcmRequest, ct);
                        fcmResponses.Add(fcmResponse);
                    }
                }
                else if (this.fcmLegacyClient != null)
                {
                    var fcmRequest = new FcmLegacyRequest()
                    {
                        RegistrationIds = fcmPushDevices.Select(d => d.DeviceToken).ToList(),
                        Notification = new FcmNotification
                        {
                            Title = pushRequest.Content.Title,
                            Body = pushRequest.Content.Body,
                        },
                        Data = pushRequest.Content.CustomData
                    };

                    var fcmResponse = await this.fcmLegacyClient.SendAsync(fcmRequest, ct);
                    fcmLegacyResponses.Add(fcmResponse);
                }
                else
                {
                    throw new InvalidOperationException("Neither an FcmClient nor a legacy FcmClient is available; this situation should not happen!");
                }
            }

            // Map platform-specific responses to platform-agnostic response
            var apnsPushResults = apnsResponses.Select(r => new PushResponseResult(r, r.Token, r.IsSuccessful));
            
            var fcmPushResults = fcmResponses.Select(r => new PushResponseResult(r, r.Token, r.IsSuccessful));

            var fcmLegacyPushResults = fcmLegacyResponses.SelectMany(r => r.Results.Select(m => new PushResponseResult(r, m.RegistrationId, isSuccessful: m.Error == null)));

            return new PushResponse(apnsPushResults.Union(fcmPushResults).Union(fcmLegacyPushResults).ToList());
        }
    }
}
