using PushNotifications.Apple;

namespace PushNotifications.Tests.Testdata
{
    internal static class ApnsJwtTestOptions
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
    }
}
