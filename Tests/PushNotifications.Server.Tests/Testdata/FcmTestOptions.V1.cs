using PushNotifications.Server.Google;

namespace PushNotifications.Server.Tests.Testdata
{
    internal static partial class FcmTestOptions
    {
        internal static class V1
        {
            internal static FcmOptions GetFcmOptions()
            {
                return new FcmOptions
                {
                    ServiceAccountKeyFilePath = ".\\Testdata\\ServiceAccountKeyFile.json",
                };
            }
        }
    }
}
