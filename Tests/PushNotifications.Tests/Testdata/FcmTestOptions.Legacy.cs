using PushNotifications.Google.Legacy;

namespace PushNotifications.Tests.Testdata
{
    internal static partial class FcmTestOptions
    {
        internal static class Legacy
        {
            internal static FcmOptions GetFcmOptions()
            {
                return new FcmOptions
                {
                    SenderId = "SenderId",
                    SenderAuthToken = "SenderAuthToken",
                    PackageName = "PackageName",
                };
            }
        }
    }
}
