using System;

namespace PushNotifications.Server.Apple
{
    public class ApplePushAlert
    {
        public string Title { get; }

        public string Subtitle { get; }

        public string Body { get; }

        public ApplePushAlert( string title, string body)
        {
            this.Title = title;
            this.Body = body ?? throw new ArgumentNullException(nameof(body));
        }

        public ApplePushAlert( string title,  string subtitle, string body)
        {
            this.Title = title;
            this.Subtitle = subtitle;
            this.Body = body ?? throw new ArgumentNullException(nameof(body));
        }
    }

}
