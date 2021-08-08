using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using PushNotifications.Server.ConsoleSample.Demodata;
using PushNotifications.Server.Google.Legacy;

namespace PushNotifications.Server.ConsoleSample
{
    partial class Program
    {
        private static async Task SendFcmLegacyPushNotification()
        {
            var sectionFcmOptions = configuration.GetSection("PushNotifications:FcmLegacyOptions");
            var fcmOptions = new FcmOptions();
            sectionFcmOptions.Bind(fcmOptions);

            IFcmClient fcmClient = new FcmClient(fcmOptions);
            var pushDevices = PushDevices.Get()
                .Where(d => d.Platform == RuntimePlatform.Android)
                .ToList();

            var fcmRequest = new FcmRequest()
            {
                RegistrationIds = pushDevices.Select(d => d.DeviceToken).ToList(),
                Notification = new FcmNotification
                {
                    Title = "Test Message",
                    Body = $"Message from PushNotifications.ConsoleSample @ {DateTime.Now}",
                },
                Data = new Dictionary<string, string>
                {
                    { "key", "value" }
                },
            };

            var fcmResponse = await fcmClient.SendAsync(fcmRequest);
            if (fcmResponse.IsSuccessful)
            {
                Console.WriteLine($"Successfully sent push notification");
            }
            else
            {
                Console.WriteLine($"Failed to send push notification");
            }
        }
    }
}
