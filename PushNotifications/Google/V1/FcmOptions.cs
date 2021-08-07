using System;

namespace PushNotifications.Google
{
    public class FcmOptions
    {
        private string project;
        private string credentials;
        private string serviceAccountKeyFilePath;

        public FcmOptions()
        {
        }

        public FcmOptions (string project, string credentials)
        {
            this.Project = project;
            this.Credentials = credentials;
        }

        public string Project
        {
            get => this.project;
            set => this.project = value ?? throw new ArgumentNullException(nameof(this.Project));
        }
        
        public string Credentials
        {
            get => this.credentials;
            set => this.credentials = value ?? throw new ArgumentNullException(nameof(this.Credentials));
        }

        public string ServiceAccountKeyFilePath
        {
            get => this.serviceAccountKeyFilePath;
            set
            {
                if (value != null && this.Credentials != null)
                {
                    throw new InvalidOperationException("Either path to the service file or service file's contents must be provided, not both.");
                }

                this.serviceAccountKeyFilePath = value;
            }
        }
    }
}

