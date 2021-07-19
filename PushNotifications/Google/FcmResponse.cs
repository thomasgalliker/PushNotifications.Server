using System.Collections.Generic;
using Newtonsoft.Json;

namespace PushNotifications.Google
{
    public class FcmResponse
    {
        public FcmResponse()
        {
            this.MulticastId = -1;
            this.NumberOfSuccesses = 0;
            this.NumberOfFailures = 0;
            this.NumberOfCanonicalIds = 0;
            this.OriginalNotification = null;
            this.Results = new List<FcmMessageResult>();
            this.ResponseCode = FcmResponseCode.Ok;
        }

        [JsonProperty("multicast_id")]
        public long MulticastId { get; set; }

        [JsonProperty("success")]
        public long NumberOfSuccesses { get; set; }

        [JsonProperty("failure")]
        public long NumberOfFailures { get; set; }

        [JsonProperty("canonical_ids")]
        public long NumberOfCanonicalIds { get; set; }

        [JsonIgnore]
        public FcmRequest OriginalNotification { get; set; }

        [JsonProperty("results")]
        public List<FcmMessageResult> Results { get; set; }

        [JsonIgnore]
        public FcmResponseCode ResponseCode { get; set; }
    }
}

