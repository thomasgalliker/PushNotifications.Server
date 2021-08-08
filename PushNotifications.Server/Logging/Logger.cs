using System;
using System.Threading;

namespace PushNotifications.Server.Logging
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
            Logger.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public static ILogger Current => logger ?? defaultLogger.Value;
    }
}