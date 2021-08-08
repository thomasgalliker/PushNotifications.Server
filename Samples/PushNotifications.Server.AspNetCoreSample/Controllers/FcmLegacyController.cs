using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PushNotifications.Server.Server.AspNetCoreSample.Demodata;
using PushNotifications.Server.Google.Legacy;

namespace PushNotifications.Server.Server.AspNetCoreSample.Controllers
{
    [ApiController]
    [Route("fcmlegacy")]
    public class FcmLegacyController : ControllerBase
    {
        private readonly ILogger<FcmLegacyController> logger;
        private readonly IFcmClient fcmClient;

        public FcmLegacyController(
            ILogger<FcmLegacyController> logger,
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
                    To = token,
                    //RegistrationIds = pushDevices.ToList(),
                    Notification = new FcmNotification
                    {
                        Title = "Test Message",
                        Body = $"Message from PushNotifications.Server.AspNetCoreSample @ {DateTime.Now}",
                    },
                    Data = new Dictionary<string, string>
                    {
                        { "key", "value" }
                    },
                };

                var fcmResponse = await this.fcmClient.SendAsync(fcmRequest);
                responses.Add(fcmResponse);

                if (fcmResponse.IsSuccessful)
                {
                    this.logger.LogInformation($"Successfully sent push notification to device {token}");
                }
                else
                {
                    this.logger.LogInformation($"Failed to send push notification to device {token}: {fcmResponse.Results[0].Error}");
                }
            }

            return responses;
        }
    }
}
