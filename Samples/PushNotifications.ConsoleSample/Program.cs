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

            try
            {
                SendPushNotificationAsync(options, "85bea18076def67319aa2345e30ca5fbce20296e2af05640cd6036c9543dbbb3").Wait();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadKey();
        }

        private static async Task SendPushNotificationAsync(ApnsJwtOptions options, string token)
        {
            IApnsClient apnsClient = new ApnsClient(new HttpClient(), options);

            var push = new ApplePush(ApplePushType.Alert)
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
                Console.WriteLine($"Failed to send push notification to device {token}: {response.ReasonString}");
            }
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Console.WriteLine($"{e.ExceptionObject}");
        }
    }
}
