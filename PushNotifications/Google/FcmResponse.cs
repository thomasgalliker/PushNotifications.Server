using System.Collections.Generic;
using Newtonsoft.Json;
using PushNotifications.Abstractions;

namespace PushNotifications.Google
{
    public class FcmResponse : IPushResponse
    {
        public FcmResponse()
        {
            this.MulticastId = -1;
            this.NumberOfSuccesses = 0;
            this.NumberOfFailures = 0;
            this.Results = new List<FcmMessageResult>();
        }

        /// <summary>
        /// Unique ID (number) identifying the multicast message.
        /// </summary>
        [JsonProperty("multicast_id")]
        public long MulticastId { get; set; }

        /// <summary>
        /// Number of messages that were processed without an error.
        /// </summary>
        [JsonProperty("success")]
        public long NumberOfSuccesses { get; set; }

        /// <summary>
        /// Number of messages that could not be processed.
        /// </summary>
        [JsonProperty("failure")]
        public long NumberOfFailures { get; set; }

        /// <summary>
        /// Array of objects representing the status of the messages processed.
        /// The objects are listed in the same order as the request
        /// (i.e., for each registration ID in the request, its result is listed in the same index in the response).
        /// </summary>
        [JsonProperty("results")]
        public List<FcmMessageResult> Results { get; set; }

        public bool IsSuccessful => this.NumberOfFailures == 0;
    }
}

