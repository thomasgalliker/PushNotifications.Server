using Microsoft.Extensions.Logging;

namespace PushNotifications.AspNetCore.Logging
{
    internal class AspNetCoreLogger : PushNotifications.Logging.ILogger
    {
        private readonly ILogger logger;

        public AspNetCoreLogger(ILogger logger)
        {
            this.logger = logger;
        }

        public void Log(PushNotifications.Logging.LogLevel level, string message)
        {
            this.logger.Log(MapLogLevel(level), message);
        }

        private static LogLevel MapLogLevel(PushNotifications.Logging.LogLevel logLevel)
        {
            switch (logLevel)
            {
                case PushNotifications.Logging.LogLevel.Info:
                    return LogLevel.Information;
                case PushNotifications.Logging.LogLevel.Warning:
                    return LogLevel.Warning;
                case PushNotifications.Logging.LogLevel.Debug:
                    return LogLevel.Debug;
                case PushNotifications.Logging.LogLevel.Error:
                    return LogLevel.Error;
                default:
                    return LogLevel.Debug;
            }
        }
    }
}