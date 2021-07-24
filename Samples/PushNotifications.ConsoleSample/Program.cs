using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using PushNotifications.Apple;
using PushNotifications.ConsoleSample.Logging;
using PushNotifications.Logging;

namespace PushNotifications.ConsoleSample
{
    class Program
    {
        private static readonly PushDevice[] pushDevices = new[]
          {
            PushDevice.Android("dBpr37I3WlI:APA91bHqhmzZVoUd2hE9Yw-s3wDOtzexg0LkDew59q0Q1hjc2a3KN0kZu0fSZpqSIej346F69q0eKm3u0WJEgG3_AOM44E3DH-AvnHM6vjIMRora-eXKyJ7kDZ5F1lpZXfNb1B0hxmeS"),
            PushDevice.Android("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA"),
            PushDevice.iOS("85bea18076def67319aa2345e30ca5fbce20296e2af05640cd6036c9543dbbb3"), // Token expired
            PushDevice.iOS("235857441ce4ad2fa491c48738dafb1e456cf5d76252967bd4ceb5a4ccb11777"), // Valid Token
            PushDevice.iOS("IIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIII"),
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

            var configurationSection = config.GetSection("PushNotification:iOS");
            var options = new ApnsJwtOptions();
            configurationSection.Bind(options);

            SendApnsPushNotification(options, "85bea18076def67319aa2345e30ca5fbce20296e2af05640cd6036c9543dbbb3").Wait();

            Console.ReadKey();
        }

        private static async Task SendApnsPushNotification(ApnsJwtOptions options, string token)
        {
            IApnsClient apnsClient = new ApnsClient(new ConsoleLogger(), new HttpClient(), options);

            var push = new ApnsRequest(ApplePushType.Alert)
                .AddToken(token)
                .AddAlert("Test Message", $"Message from PushNotifications.ConsoleSample @ {DateTime.Now}")
                .AddCustomProperty("key", "value");

            var response = await apnsClient.SendAsync(push);
            if (response.IsSuccessful)
            {
                Console.WriteLine($"Successfully sent push notification to device {token}");
            }
            else
            {
                Console.WriteLine($"Failed to send push notification to device {token}: {response.Reason}");
            }
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Console.WriteLine($"{e.ExceptionObject}");
        }
    }
}
