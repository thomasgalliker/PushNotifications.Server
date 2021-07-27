using PushNotifications.Apple;
using PushNotifications.Google;

namespace PushNotifications.AspNetCore
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
    }
}
