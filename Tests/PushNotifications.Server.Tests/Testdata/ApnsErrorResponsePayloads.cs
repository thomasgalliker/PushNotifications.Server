using System;
using PushNotifications.Server.Apple;

namespace PushNotifications.Server.Tests.Testdata
{
    internal static class ApnsErrorResponsePayloads
    {
        internal static ApnsErrorResponsePayload GetApnsErrorResponsePayload()
        {
            return new ApnsErrorResponsePayload
            {
                Reason = ApnsResponseReason.Unregistered,
                Timestamp = new DateTimeOffset(new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc))
            };
        }
    }
}
