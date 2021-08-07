using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PushNotifications.AspNetCoreSample.Demodata;

namespace PushNotifications.AspNetCoreSample.Controllers
{
    [ApiController]
    [Route("x")]
    public class XController : ControllerBase
    {
        private readonly ILogger<XController> logger;
        private readonly IPushNotificationClient pushNotificationClient;

        public XController(
            ILogger<XController> logger,
            IPushNotificationClient pushNotificationClient)
        {
            this.logger = logger;
            this.pushNotificationClient = pushNotificationClient;
        }

        [HttpGet("send")]
        public async Task<PushResponse> SendXPushNotifications()
        {
            this.logger.LogInformation("Sending push notifications...");

            var pushDevices = PushDevices.Get().ToList();

            var pushRequest = new PushRequest
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
                Devices = pushDevices
            };

            var pushResponse = await this.pushNotificationClient.SendAsync(pushRequest);

            if (pushResponse.IsSuccessful)
            {
                this.logger.LogInformation($"Successfully sent push notification to {pushResponse.Results.Count} devices");
            }
            else
            {
                // Detect all push device tokens which are expired, not registered, etc...
                // These tokens need to be removed from our push notification repository/database.
                var tokensWithRegistrationProblem = pushResponse.GetTokensWithRegistrationProblem();
                if (tokensWithRegistrationProblem.Any())
                {
                    this.logger.LogWarning(
                        $"Following device tokens have registration problems:{Environment.NewLine}" +
                        $"{string.Join(Environment.NewLine, tokensWithRegistrationProblem)}");
                }

                // PushResponse not only consolidates the received push responses
                // it also allows to evaluate the individual results (in case of a multicast request).
                foreach (var result in pushResponse.Results)
                {
                    if (result.IsSuccessful)
                    {
                        this.logger.LogInformation($"Successfully sent push notification to DeviceToken={result.DeviceToken}");
                    }
                    else
                    {
                        this.logger.LogError($"Failed to send push notification to DeviceToken={result.DeviceToken}");
                        // TODO: Log reason for error here!
                    }
                }
            }

            return pushResponse;
        }
    }
}
