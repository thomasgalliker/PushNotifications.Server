﻿using System;
using System.Diagnostics;

namespace PushNotifications.Server.Logging
{
    public class DebugLogger : ILogger
    {
        public void Log(LogLevel logLevel, string message)
        {
            Debug.WriteLine($"{DateTime.UtcNow}|PushNotifications|{logLevel}|{message}[EOL]");
        }
    }
}