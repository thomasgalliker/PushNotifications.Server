using Microsoft.Extensions.Logging;

namespace PushNotifications.AspNetCore.Logging
{
    internal class AspNetCoreLogger : PushNotifications.Logging.ILogger
    {
        private readonly Microsoft.Extensions.Logging.ILogger logger;

        public AspNetCoreLogger(Microsoft.Extensions.Logging.ILogger logger)
        {
            this.logger = logger;
        }
        public void Log(PushNotifications.Logging.LogLevel level, string message)
        {
            this.logger.Log(Microsoft.Extensions.Logging.LogLevel.Information, message);
        }
    }
}