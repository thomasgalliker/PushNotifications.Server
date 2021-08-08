using PushNotifications.Server.Apple;
using PushNotifications.Server.Google;
using FcmLegacyOptions = PushNotifications.Server.Google.Legacy.FcmOptions;

namespace PushNotifications.Server.Server.AspNetCore
{
    public class PushNotificationsOptions
    {
        /// <summary>
        /// Do not perform a server certificate validation when establishing connection with APNs.
        /// Potentially dangerous option that shouldn't be used in production.
        /// </summary>
        public bool DisableServerCertificateValidation { get; set; }

        public ApnsJwtOptions ApnsJwtOptions { get; set; }

        public FcmOptions FcmOptions { get; set; }

        public FcmLegacyOptions FcmLegacyOptions { get; set; }
    }
}
