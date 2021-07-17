using System;

namespace PushNotifications.Apple
{
    public class ApnsJwtOptions
    {
        private string certFilePath;
        private string certContent;
        private string keyId;
        private string teamId;
        private string bundleId;

        public bool UseSandbox { get; set; }

        /// <summary>
        /// Path to a .p8 certificate containing a key to be used to encrypt JWT. If specified, <see cref="CertContent"/> must be null.
        /// </summary>

        public string CertFilePath
        {
            get => this.certFilePath;
            set
            {
                if (value != null && this.CertContent != null)
                {
                    throw new InvalidOperationException("Either path to the certificate or certificate's contents must be provided, not both.");
                }

                this.certFilePath = value;
            }
        }

        /// <summary>
        /// Contents of a .p8 certificate containing a key to be used to encrypt JWT. Can include BEGIN/END headers, line breaks, etc. If specified, <see cref="CertContent"/> must be null.
        /// </summary>

        public string CertContent
        {
            get => this.certContent;
            set
            {
                if (value != null && this.CertFilePath != null)
                {
                    throw new InvalidOperationException("Either path to the certificate or certificate's contents must be provided, not both.");
                }

                this.certContent = value;
            }
        }

        /// <summary>
        /// The 10-character Key ID you obtained from your developer account. See <a href="https://developer.apple.com/documentation/usernotifications/setting_up_a_remote_notification_server/establishing_a_token-based_connection_to_apns#2943371">Reference</a>.
        /// </summary>
        public string KeyId
        {
            get => this.keyId;
            set => this.keyId = value ?? throw new ArgumentNullException(nameof(this.KeyId));
        }

        /// <summary>
        /// 10-character Team ID you use for developing your company's apps.
        /// </summary>
        public string TeamId
        {
            get => this.teamId;
            set => this.teamId = value ?? throw new ArgumentNullException(nameof(this.TeamId));
        }

        /// <summary>
        /// Your app's bundle ID.
        /// </summary>
        public string BundleId
        {
            get => this.bundleId;
            set => this.bundleId = value ?? throw new ArgumentNullException(nameof(this.BundleId));
        }
    }
}