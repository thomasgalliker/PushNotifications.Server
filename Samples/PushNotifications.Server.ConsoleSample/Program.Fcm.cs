using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using PushNotifications.Server.ConsoleSample.Demodata;
using PushNotifications.Server.Google;

namespace PushNotifications.Server.ConsoleSample
{
    partial class Program
    {
        private static async Task SendFcmPushNotification()
        {
            var sectionFcmOptions = configuration.GetSection("PushNotifications:FcmOptions");
            var fcmOptions = new FcmOptions();
            sectionFcmOptions.Bind(fcmOptions);

            IFcmClient fcmClient = new FcmClient(fcmOptions);

            var pushDevices = PushDevices.Get()
               .Where(d => d.Platform == RuntimePlatform.Android)
               .ToList();

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
                            Body = $"Message from PushNotifications.ConsoleSample @ {DateTime.Now}",
                        },
                        Data = new Dictionary<string, string>
                        {
                            { "key", "value" }
                        },
                    },
                    ValidateOnly = false,
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
}
