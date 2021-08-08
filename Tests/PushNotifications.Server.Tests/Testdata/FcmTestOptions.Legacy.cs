using PushNotifications.Server.Google.Legacy;

namespace PushNotifications.Server.Tests.Testdata
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
