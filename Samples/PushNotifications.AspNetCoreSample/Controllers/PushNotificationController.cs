using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PushNotifications.Apple;
using PushNotifications.AspNetCore;

namespace PushNotifications.AspNetCoreSample.Controllers
{
    [ApiController]
    [Route("pushnotification")]
    public class PushNotificationController : ControllerBase
    {
        private static readonly string[] tokens = new[]
        {
            "85bea18076def67319aa2345e30ca5fbce20296e2af05640cd6036c9543dbbb3",
            "0000000000000000000000000000000000000000000000000000000000000000",
        };

        private readonly ILogger<PushNotificationController> logger;
        private readonly IApnsService apnsService;

        public PushNotificationController(
            ILogger<PushNotificationController> logger,
            IApnsService apnsService)
        {
            this.logger = logger;
            this.apnsService = apnsService;
        }

        [HttpGet("test")]
        public async Task<IEnumerable<ApnsResponse>> TestAsync()
        {
            this.logger.LogInformation("Sending test push notifications...");

            var responses = new List<ApnsResponse>(tokens.Length);

            foreach (var token in tokens)
            {
                var push = new ApplePush(ApplePushType.Alert)
                     .AddToken(token)
                     .AddAlert("Test Message", $"Message from PushNotifications.AspNetCoreSample @ {DateTime.Now}")
                     .AddCustomProperty("key", "value");

                var response = await this.apnsService.SendAsync(push);
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
    }
}
