using System;

namespace PushNotifications.Server.Apple
{
    public class ApnsCertificateExpiredException : Exception
    {
        const string ConstMessage = "Your APNs certificate has expired. Please renew it. More info: https://developer.apple.com/documentation/usernotifications/setting_up_a_remote_notification_server/establishing_a_certificate-based_connection_to_apns";

        internal ApnsCertificateExpiredException( Exception innerException) : base(ConstMessage, innerException)
        {
        }
    }
}
