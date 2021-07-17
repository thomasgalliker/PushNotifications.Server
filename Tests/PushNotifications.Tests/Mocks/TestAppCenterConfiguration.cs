using System.Collections.Generic;

namespace PushNotifications.Tests.Mocks
{
    public class TestAppCenterConfiguration : IAppCenterConfiguration
    {
        public const string AppNameAndroid = "TestApp.Android";
        public const string AppNameiOS = "TestApp.iOS";

        public string OrganizationName { get; } = "testOrg";

        public IDictionary<RuntimePlatform, string> AppNames { get; } = new Dictionary<RuntimePlatform, string>
        {
            { RuntimePlatform.Android, AppNameAndroid },
            { RuntimePlatform.iOS, AppNameiOS },
        };

        public string ApiToken { get; } = "testToken";
    }
}