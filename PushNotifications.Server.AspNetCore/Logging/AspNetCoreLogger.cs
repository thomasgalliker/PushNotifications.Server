using Microsoft.Extensions.Logging;

namespace PushNotifications.Server.AspNetCore.Logging
{
    internal class AspNetCoreLogger : PushNotifications.Server.Logging.ILogger
    {
        private readonly ILogger logger;

        public AspNetCoreLogger(ILogger logger)
        {
            this.logger = logger;
        }

        public void Log(PushNotifications.Server.Logging.LogLevel level, string message)
        {
            this.logger.Log(MapLogLevel(level), message);
        }

        private static LogLevel MapLogLevel(PushNotifications.Server.Logging.LogLevel logLevel)
        {
            switch (logLevel)
            {
                case PushNotifications.Server.Logging.LogLevel.Info:
                    return LogLevel.Information;
                case PushNotifications.Server.Logging.LogLevel.Warning:
                    return LogLevel.Warning;
                case PushNotifications.Server.Logging.LogLevel.Debug:
                    return LogLevel.Debug;
                case PushNotifications.Server.Logging.LogLevel.Error:
                    return LogLevel.Error;
                default:
                    return LogLevel.Debug;
            }
        }
    }
}