using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PushNotifications.Apple;
using PushNotifications.AspNetCore;
using PushNotifications.Google;

namespace PushNotifications.AspNetCoreSample.Controllers
{
    [ApiController]
    [Route("pushnotification")]
    public class PushNotificationController : ControllerBase
    {
        private static readonly PushDevice[] pushDevices = new[]
        {
            //PushDevice.Android("dBpr37I3WlI:APA91bHqhmzZVoUd2hE9Yw-s3wDOtzexg0LkDew59q0Q1hjc2a3KN0kZu0fSZpqSIej346F69q0eKm3u0WJEgG3_AOM44E3DH-AvnHM6vjIMRora-eXKyJ7kDZ5F1lpZXfNb1B0hxmeS"),
            //PushDevice.Android("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA"),
            //PushDevice.iOS("85bea18076def67319aa2345e30ca5fbce20296e2af05640cd6036c9543dbbb3"), // Token expired
            PushDevice.iOS("0537210f8177b2cf14c3c8bd43d7f95e23319f21cf0d84327eef174c61e42fce"), // Valid Token
            //PushDevice.iOS("IIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIII"),
        };

        private readonly ILogger<PushNotificationController> logger;
        private readonly IApnsService apnsService;
        private readonly IFcmService fcmService;
        private readonly IPushNotificationService pushNotificationService;

        public PushNotificationController(
            ILogger<PushNotificationController> logger,
            IApnsService apnsService,
            IFcmService fcmService,
            IPushNotificationService pushNotificationService)
        {
            this.logger = logger;
            this.apnsService = apnsService;
            this.fcmService = fcmService;
            this.pushNotificationService = pushNotificationService;
        }

        [HttpGet("send/apns")]
        public async Task<IEnumerable<ApnsResponse>> SendApnsPushNotifications()
        {
            this.logger.LogInformation("Sending APNS push notifications...");

            var responses = new List<ApnsResponse>(pushDevices.Length);

            foreach (var pushDevice in pushDevices)
            {
                var token = pushDevice.DeviceToken;

                var request = new ApnsRequest(ApplePushType.Alert)
                     .AddToken(token)
                     .AddAlert("Test Message", $"Message from PushNotifications.AspNetCoreSample @ {DateTime.Now}")
                     .AddCustomProperty("key", "value");

                var response = await this.apnsService.SendAsync(request);
                responses.Add(response);

                if (response.IsSuccessful)
                {
                    this.logger.LogInformation($"Successfully sent push notification to device {token}");
                }
                else
                {
                    this.logger.LogInformation($"Failed to send push notification to device {token}: {response.Reason}");
                }
            }

            return responses;
        }

        [HttpGet("send/fcm")]
        public async Task<IEnumerable<FcmResponse>> SendFcmPushNotifications()
        {
            this.logger.LogInformation("Sending FCM push notifications...");

            var responses = new List<FcmResponse>(pushDevices.Length);

            foreach (var pushDevice in pushDevices)
            {
                var token = pushDevice.DeviceToken;

                var request = new FcmRequest()
                {
                    To = token,
                    //RegistrationIds = pushDevices.ToList(),
                    Notification = new FcmNotification
                    {
                        Title = "Test Message",
                        Body = $"Message from PushNotifications.AspNetCoreSample @ {DateTime.Now}",
                    },
                    Data = new Dictionary<string, string>
                    {
                        { "key", "value" }
                    },
                };

                var response = await this.fcmService.SendAsync(request);
                responses.Add(response);

                if (response.IsSuccessful)
                {
                    this.logger.LogInformation($"Successfully sent push notification to device {token}");
                }
                else
                {
                    this.logger.LogInformation($"Failed to send push notification to device {token}: {response.Results[0].Error}");
                }
            }

            return responses;
        }

        [HttpGet("send/x")]
        public async Task<IEnumerable<PushResponse>> SendXPushNotifications()
        {
            this.logger.LogInformation("Sending push notifications...");

            var responses = new List<PushResponse>(pushDevices.Length);

            foreach (var pushDevice in pushDevices)
            {
                var request = new PushRequest
                {
                    Content = new PushContent
                    {
                        Title = "Test Message",
                        Body = $"Message from PushNotifications.AspNetCoreSample @ {DateTime.Now}",
                        CustomData = new Dictionary<string, string>
                        {
                            { "key", "value" }
                        }
                    },
                    Devices = new List<PushDevice> { pushDevice }
                };

                var response = await this.pushNotificationService.SendAsync(request);
                responses.Add(response);

                //if (response.IsSuccessful)
                //{
                //    this.logger.LogInformation($"Successfully sent push notification to device {token}");
                //}
                //else
                //{
                //    this.logger.LogInformation($"Failed to send push notification to device {token}: {response.Results[0].Error}");
                //}
            }

            return responses;
        }
    }
}
