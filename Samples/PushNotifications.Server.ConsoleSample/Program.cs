using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using PushNotifications.Server.ConsoleSample.Logging;
using PushNotifications.Server.Logging;

namespace PushNotifications.Server.ConsoleSample
{
    partial class Program
    {
        private static IConfigurationRoot configuration;

        static async Task Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            Console.WriteLine($"PushNotifications.ConsoleSample [Version 1.0.0.0]");
            Console.WriteLine($"(c) 2021 superdev gmbh. All rights reserved.");
            Console.WriteLine();

            Logger.SetLogger(new ConsoleLogger());

            configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .Build();

            // Sending push notifications to iOS devices
            await SendApnsPushNotification();

            // Sending push notifications to Android devices (FCM V1 HTTP API)
            await SendFcmPushNotification();

            // Sending push notifications to Android devices (FCM Legacy HTTP API)
            await SendFcmLegacyPushNotification();

            // Sending push notifications to all platforms
            await SendXPushNotification();

            Console.WriteLine();
            Console.WriteLine("Press any key to close this window...");
            Console.ReadKey();
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Console.WriteLine($"{e.ExceptionObject}");
        }
    }
}
