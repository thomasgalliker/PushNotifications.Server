using System;
using System.Threading;

namespace PushNotifications.Logging
{
    public static class Logger
    {
        private static readonly Lazy<ILogger> defaultLogger = new Lazy<ILogger>(CreateDefaultLogger, LazyThreadSafetyMode.PublicationOnly);
        private static ILogger logger;

        private static ILogger CreateDefaultLogger()
        {
            return new DebugLogger();
        }

        public static void SetLogger(ILogger logger)
        {
            Logger.logger = logger;
        }

        public static ILogger Current => logger ?? defaultLogger.Value;

        public static void Debug(string message)
        {
            Current.Log(LogLevel.Debug, message);
        }

        public static void Info(string message)
        {
            Current.Log(LogLevel.Info, message);
        }

        public static void Warning(string message)
        {
            Current.Log(LogLevel.Warning, message);
        }

        public static void Error(string message)
        {
            Current.Log(LogLevel.Error, message);
        }

        public static void Error(string message, Exception ex)
        {
            Error(message + $" {ex.Message} {Environment.NewLine} {ex.StackTrace}");
        }
    }
}