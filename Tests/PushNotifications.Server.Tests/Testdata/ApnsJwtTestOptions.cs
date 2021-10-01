using System.IO;
using PushNotifications.Server.Apple;

namespace PushNotifications.Server.Tests.Testdata
{
    internal static class ApnsJwtTestOptions
    {
        internal static ApnsJwtOptions GetApnsJwtOptions()
        {
            return new ApnsJwtOptions
            {
                BundleId = "BundleId",
                CertFilePath = Path.Combine(".", "Testdata", "AuthKey_QWWBGJLG7F.p8"), // https://github.com/search?q=private+extension%3Ap8&type=code
                KeyId = "KeyId",
                TeamId = "TeamId",
                UseSandbox = true,
            };
        }
    }
}
