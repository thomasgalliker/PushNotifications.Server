using System.Collections.Generic;
using System.Linq;

namespace PushNotifications
{
    /// <summary>
    /// Cross-platform abstraction of a push response.
    /// </summary>
    public class PushResponse : IPushResponse
    {
        internal PushResponse(ICollection<PushResult> results)
        {
            this.Results = results ?? new List<PushResult>();
        }

        public ICollection<PushResult> Results { get; }

        public bool IsSuccessful => !this.Results.Any(r => r.IsSuccessful == false);
    }

    public class PushResult
    {
        public bool IsSuccessful { get; set; }

        public string DeviceToken { get; set; }

        public IPushResponse OriginalResponse { get; set; }
    }
}