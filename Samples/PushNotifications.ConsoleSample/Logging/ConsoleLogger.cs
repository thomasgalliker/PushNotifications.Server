using System;
using PushNotifications.Logging;

namespace PushNotifications.ConsoleSample.Logging
{
    public class ConsoleLogger : ILogger
    {
        public void Log(LogLevel level, string message)
        {
            Console.WriteLine($"{DateTime.Now}|{level}|{message}");
        }
    }
}