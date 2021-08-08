using System;
using PushNotifications.Server.Logging;
using Xunit.Abstractions;

namespace PushNotifications.Server.Tests.Utils
{
    public class TestOutputHelperLogger<T> : ILogger
    {
        private readonly ITestOutputHelper testOutputHelper;

        public TestOutputHelperLogger(ITestOutputHelper testOutputHelper)
        {
            this.Name = typeof(T).Name;
            this.testOutputHelper = testOutputHelper;
        }

        public string Name { get; }

        public void Log(LogLevel logLevel, string message)
        {
            try
            {
                this.testOutputHelper.WriteLine($"{DateTime.UtcNow}|{this.Name}|{logLevel}|{message}[EOL]");
            }
            catch (InvalidOperationException)
            {
                // TestOutputHelperLogger throws an InvalidOperationException
                // if it is no longer associated with a test case.
            }
        }
    }
}
