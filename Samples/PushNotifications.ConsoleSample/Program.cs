using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using PushNotifications.Apple;
using PushNotifications.ConsoleSample.Logging;
using PushNotifications.Google;
using PushNotifications.Logging;

namespace PushNotifications.ConsoleSample
{
    class Program
    {
        private static readonly PushDevice[] pushDevices = new[]
          {
            PushDevice.Android("dBpr37I3WlI:APA91bHqhmzZVoUd2hE9Yw-s3wDOtzexg0LkDew59q0Q1hjc2a3KN0kZu0fSZpqSIej346F69q0eKm3u0WJEgG3_AOM44E3DH-AvnHM6vjIMRora-eXKyJ7kDZ5F1lpZXfNb1B0hxmeS"),
            PushDevice.iOS("235857441ce4ad2fa491c48738dafb1e456cf5d76252967bd4ceb5a4ccb11777"),
        };

        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            Console.WriteLine($"PushNotifications.ConsoleSample [Version 1.0.0.0]");
            Console.WriteLine($"(c) 2021 superdev gmbh. All rights reserved.");
            Console.WriteLine();

            Logger.SetLogger(new ConsoleLogger());

            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .Build();

            var sectionApnsJwtOptions = config.GetSection("PushNotifications:ApnsJwtOptions");
            var apnsJwtOptions = new ApnsJwtOptions();
            sectionApnsJwtOptions.Bind(apnsJwtOptions);

            var sectionFcmOptions = config.GetSection("PushNotifications:FcmOptions");
            var fcmOptions = new FcmOptions();
            sectionFcmOptions.Bind(fcmOptions);

            IApnsClient apnsClient = new ApnsClient(apnsJwtOptions);

            IFcmClient fcmClient = new FcmClient(fcmOptions);

            // Sending push notifications to iOS devices
            SendApnsPushNotification(apnsClient).Wait();

            // Sending push notifications to Android devices
            SendFcmPushNotification(fcmClient).Wait();

            // Sending push notifications to all platforms
            SendXPushNotifications(fcmClient, apnsClient).Wait();

            Console.ReadKey();
        }

        private static async Task<PushResponse> SendXPushNotifications(IFcmClient fcmClient, IApnsClient apnsClient)
        {
            var pushNotificationClient = new PushNotificationClient(new ConsoleLogger(), fcmClient, apnsClient);

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

            return pushResponse;
        }

        private static async Task SendApnsPushNotification(IApnsClient apnsClient)
        {
            foreach (var pushDevice in pushDevices.Where(d => d.Platform == RuntimePlatform.iOS))
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

        private static async Task SendFcmPushNotification(IFcmClient fcmClient)
        {
            var fcmRequest = new FcmRequest()
            {
                RegistrationIds = pushDevices.Where(d => d.Platform == RuntimePlatform.Android).Select(d => d.DeviceToken).ToList(),
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

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Console.WriteLine($"{e.ExceptionObject}");
        }
    }
}
