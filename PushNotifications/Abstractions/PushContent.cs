using System.Collections.Generic;

namespace PushNotifications
{
    public class PushContent
    {
        public PushContent()
        {
            this.CustomData = new Dictionary<string, string>();
        }

        public string Name { get; set; }

        public string Title { get; set; }

        public string Body { get; set; }

        public IDictionary<string, string> CustomData { get; set; }
    }
}