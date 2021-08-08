using PushNotifications.Google;

namespace PushNotifications.Tests.Testdata
{
    internal static partial class FcmResponses
    {
        internal static class V1
        {
            internal static FcmResponse GetFcmResponse_Success()
            {
                return new FcmResponse
                {
                    Name = "name",
                    Token = "token",
                    Error = null,
                };
            }

            internal static FcmResponse GetFcmResponse_Error()
            {
                return new FcmResponse
                {
                    Name = null,
                    Token = "token",
                    Error = new FcmError
                    {
                        Code = 400,
                        Message = "Request contains an invalid argument.",
                        Status = FcmErrorCode.InvalidArgument,
                    },
                };
            }
        }
        
    }
}
