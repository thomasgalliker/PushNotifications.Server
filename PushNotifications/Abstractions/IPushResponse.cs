using System.Collections.Generic;

namespace PushNotifications
{
    public interface IPushResponse
    {
        /// <summary>
        /// Indicates if the push request was processed with success - or returned an error.
        /// </summary>
        bool IsSuccessful { get; }

        /// <summary>
        /// Returns a list of push tokens which have registration related problems.
        /// This can either be a missing or expired registration, but also an invalid registration token.
        /// </summary>
        IEnumerable<string> GetTokensWithRegistrationProblem();
    }
}