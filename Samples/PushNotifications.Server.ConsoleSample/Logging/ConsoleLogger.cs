using System;
using PushNotifications.Server.Logging;

namespace PushNotifications.Server.ConsoleSample.Logging
{
    public class ConsoleLogger : ILogger
    {
        public void Log(LogLevel level, string message)
        {
            Console.WriteLine($"{DateTime.Now}|{level}|{message}");
        }
    }
}