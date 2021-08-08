using System;

namespace PushNotifications.Server.Google
{
    public class FcmOptions
    {
        private string serviceAccountKeyFilePath;

        public FcmOptions()
        {
        }

        public string ServiceAccountKeyFilePath
        {
            get => this.serviceAccountKeyFilePath;
            set => this.serviceAccountKeyFilePath = value ?? throw new ArgumentException($"ServiceAccountKeyFilePath must not be null or empty", $"{nameof(this.ServiceAccountKeyFilePath)}");
        }
    }
}

