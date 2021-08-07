using System.Collections.Generic;
using PushNotifications.Google.Legacy;

namespace PushNotifications.Tests.Testdata
{
    internal static class FcmResponses
    {
        internal static FcmResponse GetFcmResponse_Success()
        {
            return new FcmResponse
            {
                MulticastId = 1L,
                NumberOfSuccesses = 1,
                NumberOfFailures = 0,
                Results = new List<FcmMessageResult>
                {
                    new FcmMessageResult
                    {
                        MessageId = "1",
                        Error = null,
                    }
                }
            };
        }

        internal static FcmResponse GetFcmResponse_Error()
        {
            return new FcmResponse
            {
                MulticastId = 2L,
                NumberOfSuccesses = 0,
                NumberOfFailures = 1,
                Results = new List<FcmMessageResult>
                {
                    new FcmMessageResult
                    {
                        Error = FcmResponseStatus.NotRegistered
                    }
                }
            };
        }
    }
}
