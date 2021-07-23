using System.Collections.Generic;
using System.Diagnostics;

namespace PushNotifications
{
    [DebuggerDisplay("PushContent: Title={this.Title}, Body={this.Body}")]
    public class PushContent
    {
        public PushContent()
        {
            this.CustomData = new Dictionary<string, string>();
        }

        public string Title { get; set; }

        public string Body { get; set; }

        public IDictionary<string, string> CustomData { get; set; }
    }
}