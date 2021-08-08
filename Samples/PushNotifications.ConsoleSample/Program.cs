using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using PushNotifications.ConsoleSample.Logging;
using PushNotifications.Logging;

namespace PushNotifications.ConsoleSample
{
    partial class Program
    {
        private static IConfigurationRoot configuration;

        static void Main(string[] args)
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
            SendApnsPushNotification().Wait();

            // Sending push notifications to Android devices (FCM V1 HTTP API)
            SendFcmPushNotification().Wait();

            // Sending push notifications to Android devices (FCM Legacy HTTP API)
            SendFcmLegacyPushNotification().Wait();

            // Sending push notifications to all platforms
            SendXPushNotification().Wait();

            Console.ReadKey();
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Console.WriteLine($"{e.ExceptionObject}");
        }
    }
}
