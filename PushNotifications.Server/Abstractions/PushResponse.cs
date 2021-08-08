using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace PushNotifications.Server
{
    /// <summary>
    /// Cross-platform abstraction of a push response.
    /// </summary>
    [DebuggerDisplay("PushResponse: IsSuccessful={this.IsSuccessful}, Results={this.Results.Count}")]
    public class PushResponse : IPushResponse
    {
        public PushResponse(ICollection<PushResponseResult> results)
        {
            this.Results = results ?? throw new ArgumentNullException(nameof(results));
        }

        public ICollection<PushResponseResult> Results { get; }

        public bool IsSuccessful => !this.Results.Any(r => r.IsSuccessful == false);

        public IEnumerable<string> GetTokensWithRegistrationProblem()
        {
            return this.Results.SelectMany(r => r.OriginalResponse.GetTokensWithRegistrationProblem());
        }
    }
}