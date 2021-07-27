using System;

namespace PushNotifications.Google
{
    public class FcmOptions
    {
        private const string FcmSendUrl = "https://fcm.googleapis.com/fcm/send";

        private string senderId;
        private string senderAuthToken;
        private string packageName;
        private string fcmUrl;

        public FcmOptions()
        {
            this.FcmUrl = FcmSendUrl;
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

        public string FcmUrl
        {
            get => this.fcmUrl;
            set => this.fcmUrl = value ?? throw new ArgumentNullException(nameof(this.FcmUrl));
        }
    }
}

