using System;

namespace PushNotifications.Server.Google.Legacy
{
    public class FcmOptions
    {
        private string senderId;
        private string senderAuthToken;
        private string packageName;

        public FcmOptions()
        {
        }

        public FcmOptions (string senderAuthToken) : this()
        {
            this.SenderAuthToken = senderAuthToken;
        }

        public FcmOptions (string senderId, string senderAuthToken, string packageName) : this(senderAuthToken)
        {
            this.SenderId = senderId;
            this.PackageName = packageName;
        }

        public string SenderId
        {
            get => this.senderId;
            set => this.senderId = value ?? throw new ArgumentNullException(nameof(this.SenderId));
        }
        
        public string SenderAuthToken
        {
            get => this.senderAuthToken;
            set => this.senderAuthToken = value ?? throw new ArgumentNullException(nameof(this.SenderAuthToken));
        }

        public string PackageName
        {
            get => this.packageName;
            set => this.packageName = value ?? throw new ArgumentNullException(nameof(this.PackageName));
        }
    }
}

