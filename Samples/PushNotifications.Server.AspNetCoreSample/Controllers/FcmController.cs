using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PushNotifications.Server.AspNetCoreSample.Demodata;
using PushNotifications.Server.Google;

namespace PushNotifications.Server.Server.AspNetCoreSample.Controllers
{
    [ApiController]
    [Route("fcm")]
    public class FcmController : ControllerBase
    {
        private readonly ILogger<FcmController> logger;
        private readonly IFcmClient fcmClient;

        public FcmController(
            ILogger<FcmController> logger,
            IFcmClient fcmClient)
        {
            this.logger = logger;
            this.fcmClient = fcmClient;
        }

        [HttpGet("send")]
        public async Task<IEnumerable<FcmResponse>> SendFcmPushNotifications()
        {
            this.logger.LogInformation("Sending FCM push notifications...");

            var pushDevices = PushDevices.Get().ToList();
            var responses = new List<FcmResponse>(pushDevices.Count);

            foreach (var pushDevice in pushDevices)
            {
                var token = pushDevice.DeviceToken;

                var fcmRequest = new FcmRequest()
                {
                    Message = new Message
                    {
                        Token = token,
                        Notification = new Notification
                        {
                            Title = "Test Message",
                            Body = $"Message from PushNotifications.Server.AspNetCoreSample @ {DateTime.Now}",
                        },
                        Data = new Dictionary<string, string>
                        {
                            { "key", "value" }
                        },
                    },
                    ValidateOnly = false,
                };

                var fcmResponse = await this.fcmClient.SendAsync(fcmRequest);
                responses.Add(fcmResponse);

                if (fcmResponse.IsSuccessful)
                {
                    this.logger.LogInformation($"Successfully sent push notification to device {token}");
                }
                else
                {
                    this.logger.LogInformation($"Failed to send push notification to device {token}: {fcmResponse.Error.Message} ({fcmResponse.Error.Code})");
                }
            }

            return responses;
        }
    }
}
