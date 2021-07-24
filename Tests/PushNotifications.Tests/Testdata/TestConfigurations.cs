using PushNotifications.Apple;
using PushNotifications.Google;

namespace PushNotifications.Tests.Testdata
{
    internal static class TestConfigurations
    {
        internal static ApnsJwtOptions GetApnsJwtOptions()
        {
            return new ApnsJwtOptions
            {
                BundleId = "BundleId",
                CertFilePath = ".\\Testdata\\AuthKey_QWWBGJLG7F.p8",
                KeyId = "KeyId",
                TeamId = "TeamId",
                UseSandbox = true,
            };
        }

        internal static FcmConfiguration GetFcmConfiguration()
        {
            return new FcmConfiguration
            {
                SenderId = "SenderId",
                SenderAuthToken = "SenderAuthToken",
                PackageName = "PackageName",
            };
        }
    }
}
