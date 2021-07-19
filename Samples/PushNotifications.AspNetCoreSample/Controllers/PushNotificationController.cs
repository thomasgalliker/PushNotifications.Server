using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using PushNotifications.Apple;
using PushNotifications.AspNetCore;
using PushNotifications.Google;

namespace PushNotifications.AspNetCoreSample.Controllers
{
    [ApiController]
    [Route("pushnotification")]
    public class PushNotificationController : ControllerBase
    {
        private static readonly string[] pushDevices = new[]
        {
            //"85bea18076def67319aa2345e30ca5fbce20296e2af05640cd6036c9543dbbb3", // iOS 
            "dBpr37I3WlI:APA91bHqhmzZVoUd2hE9Yw-s3wDOtzexg0LkDew59q0Q1hjc2a3KN0kZu0fSZpqSIej346F69q0eKm3u0WJEgG3_AOM44E3DH-AvnHM6vjIMRora-eXKyJ7kDZ5F1lpZXfNb1B0hxmeS", // Android
            "fDZq-_Suri4:APA91bEnXX3cxvCN3sut5xE1xjnfLlfF5RgVYTDHcNfvidSfW3Uhg8mJO-XTWAErwW5_doSALdMdSr9TtVfAERUJQ245SBpmudn8GJfrVOcKJkFKPIR0PuqTOevBkBVga0dSSuEozUE2", // Android
            "0000000000000000000000000000000000000000000000000000000000000000", // invalid
        };

        private readonly ILogger<PushNotificationController> logger;
        private readonly IApnsService apnsService;
        private readonly IFcmService fcmService;

        public PushNotificationController(
            ILogger<PushNotificationController> logger,
            IApnsService apnsService,
            IFcmService fcmService)
        {
            this.logger = logger;
            this.apnsService = apnsService;
            this.fcmService = fcmService;
        }

        [HttpGet("send/apns")]
        public async Task<IEnumerable<ApnsResponse>> SendApnsPushNotifications()
        {
            this.logger.LogInformation("Sending APNS push notifications...");

            var responses = new List<ApnsResponse>(pushDevices.Length);

            foreach (var token in pushDevices)
            {
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
                    this.logger.LogInformation($"Failed to send push notification to device {token}: {response.ReasonString}");
                }
            }

            return responses;
        }

        [HttpGet("send/fcm")]
        public async Task<IEnumerable<FcmResponse>> SendFcmPushNotifications()
        {
            this.logger.LogInformation("Sending FCM push notifications...");

            var responses = new List<FcmResponse>(pushDevices.Length);

            foreach (var token in pushDevices)
            {
                var request = new FcmRequest()
                {
                    //To = token,
                    RegistrationIds = new List<string> { token },
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

                //if (response.IsSuccessful)
                //{
                //    this.logger.LogInformation($"Successfully sent push notification to device {token}");
                //}
                //else
                //{
                //    this.logger.LogInformation($"Failed to send push notification to device {token}: {response.ReasonString}");
                //}
            }

            return responses;
        }
    }
}
