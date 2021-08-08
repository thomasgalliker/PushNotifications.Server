using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using PushNotifications.Server.Apple;
using PushNotifications.Server.ConsoleSample.Demodata;
using PushNotifications.Server.Google;

namespace PushNotifications.Server.ConsoleSample
{
    partial class Program
    {
        private static async Task SendXPushNotification()
        {
            var sectionApnsJwtOptions = configuration.GetSection("PushNotifications:ApnsJwtOptions");
            var apnsJwtOptions = new ApnsJwtOptions();
            sectionApnsJwtOptions.Bind(apnsJwtOptions);

            var sectionFcmOptions = configuration.GetSection("PushNotifications:FcmOptions");
            var fcmOptions = new FcmOptions();
            sectionFcmOptions.Bind(fcmOptions);

            IApnsClient apnsClient = new ApnsClient(apnsJwtOptions);
            IFcmClient fcmClient = new FcmClient(fcmOptions);
            IPushNotificationClient pushNotificationClient = new PushNotificationClient(fcmClient, apnsClient);

            var pushDevices = PushDevices.Get()
               .ToList();

            var pushRequest = new PushRequest
            {
                Content = new PushContent
                {
                    Title = "Test Message",
                    Body = $"Message from PushNotifications.ConsoleSample @ {DateTime.Now}",
                    CustomData = new Dictionary<string, string>
                    {
                        { "key", "value" }
                    }
                },
                Devices = pushDevices
            };

            var pushResponse = await pushNotificationClient.SendAsync(pushRequest);
            if (pushResponse.IsSuccessful)
            {
                Console.WriteLine($"Successfully sent push notification to {pushResponse.Results.Count} devices");
            }
            else
            {
                // Detect all push device tokens which are expired, not registered, etc...
                // These tokens need to be removed from our push notification repository/database.
                var tokensWithRegistrationProblem = pushResponse.GetTokensWithRegistrationProblem();
                if (tokensWithRegistrationProblem.Any())
                {
                    Console.WriteLine();
                    Console.WriteLine(
                        $"Following device tokens have registration problems:{Environment.NewLine}" +
                        $"{string.Join(Environment.NewLine, tokensWithRegistrationProblem)}");
                }

                Console.WriteLine();
                // PushResponse not only consolidates the received push responses
                // it also allows to evaluate the individual results (in case of a multicast request).
                foreach (var result in pushResponse.Results)
                {
                    if (result.IsSuccessful)
                    {
                        Console.WriteLine($"Successfully sent push notification to DeviceToken={result.DeviceToken}");
                    }
                    else
                    {
                        Console.WriteLine($"Failed to send push notification to DeviceToken={result.DeviceToken}"); // TODO: Log reason for error here!
                    }
                }
            }
        }

    }
}
