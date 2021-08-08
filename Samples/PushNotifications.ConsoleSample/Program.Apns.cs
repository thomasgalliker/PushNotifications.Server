using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using PushNotifications.Apple;
using PushNotifications.ConsoleSample.Demodata;

namespace PushNotifications.ConsoleSample
{
    partial class Program
    {
        private static async Task SendApnsPushNotification()
        {
            var sectionApnsJwtOptions = configuration.GetSection("PushNotifications:ApnsJwtOptions");
            var apnsJwtOptions = new ApnsJwtOptions();
            sectionApnsJwtOptions.Bind(apnsJwtOptions);

            IApnsClient apnsClient = new ApnsClient(apnsJwtOptions);

            var pushDevices = PushDevices.Get()
               .Where(d => d.Platform == RuntimePlatform.iOS)
               .ToList();

            foreach (var pushDevice in pushDevices)
            {
                var token = pushDevice.DeviceToken;

                var apnsRequest = new ApnsRequest(ApplePushType.Alert)
                    .AddToken(pushDevice.DeviceToken)
                    .AddAlert("Test Message", $"Message from PushNotifications.ConsoleSample @ {DateTime.Now}")
                    .AddCustomProperty("key", "value");

                var apnsResponse = await apnsClient.SendAsync(apnsRequest);
                if (apnsResponse.IsSuccessful)
                {
                    Console.WriteLine($"Successfully sent push notification to device {token}");
                }
                else
                {
                    Console.WriteLine($"Failed to send push notification to device {token}: {apnsResponse.Reason}");
                }
            }
        }

    }
}
