using System.Collections.Generic;

namespace PushNotifications
{
    public interface IAppCenterConfiguration
    {
        string OrganizationName { get; }

        IDictionary<RuntimePlatform, string> AppNames { get; }

        string ApiToken { get; }
    }
}