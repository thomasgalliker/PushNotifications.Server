using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace PushNotifications
{
    /// <summary>
    /// Cross-platform abstraction of a push response.
    /// </summary>
    [DebuggerDisplay("PushResponse: IsSuccessful={this.IsSuccessful}, Results={this.Results.Count}")]
    public class PushResponse : IPushResponse
    {
        internal PushResponse(ICollection<PushResponseResult> results)
        {
            this.Results = results ?? new List<PushResponseResult>();
        }

        public ICollection<PushResponseResult> Results { get; }

        public bool IsSuccessful => !this.Results.Any(r => r.IsSuccessful == false);
    }
}