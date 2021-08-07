using PushNotifications.Google.Legacy;

namespace PushNotifications.Tests.Testdata
{
    internal static class FcmTestOptions
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
