using System.Collections.Generic;
using System.Diagnostics;

namespace PushNotifications
{
    /// <summary>
    /// Cross-platform abstraction of a push request.
    /// </summary>
    [DebuggerDisplay("PushRequest: Title={this.Content.Title}, Devices={this.Devices.Count}")]
    public class PushRequest : IPushRequest
    {
        public PushRequest()
        {
            this.Devices = new List<PushDevice>();
        }

        public ICollection<PushDevice> Devices { get; set; }

        public PushContent Content { get; set; }
    }
}