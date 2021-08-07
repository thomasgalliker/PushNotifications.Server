using PushNotifications.Google;

namespace PushNotifications.Tests.Testdata
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
