using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PushNotifications.Apple;
using PushNotifications.AspNetCoreSample.Demodata;

namespace PushNotifications.AspNetCoreSample.Controllers
{
    [ApiController]
    [Route("apns")]
    public class ApnsController : ControllerBase
    {
        private readonly ILogger<ApnsController> logger;
        private readonly IApnsClient apnsClient;

        public ApnsController(
            ILogger<ApnsController> logger,
            IApnsClient apnsClient)
        {
            this.logger = logger;
            this.apnsClient = apnsClient;
        }

        [HttpGet("send")]
        public async Task<IEnumerable<ApnsResponse>> SendApnsPushNotifications()
        {
            this.logger.LogInformation("Sending APNS push notifications...");

            var pushDevices = PushDevices.Get().ToList();
            var responses = new List<ApnsResponse>(pushDevices.Count);

            foreach (var pushDevice in pushDevices)
            {
                var token = pushDevice.DeviceToken;

                var apnsRequest = new ApnsRequest(ApplePushType.Alert)
                     .AddToken(token)
                     .AddAlert("Test Message", $"Message from PushNotifications.AspNetCoreSample @ {DateTime.Now}")
                     .AddCustomProperty("key", "value");

                var apnsResponse = await this.apnsClient.SendAsync(apnsRequest);
                responses.Add(apnsResponse);

                if (apnsResponse.IsSuccessful)
                {
                    this.logger.LogInformation($"Successfully sent push notification to device {token}");
                }
                else
                {
                    this.logger.LogInformation($"Failed to send push notification to device {token}: {apnsResponse.Reason}");
                }
            }

            return responses;
        }
    }
}
