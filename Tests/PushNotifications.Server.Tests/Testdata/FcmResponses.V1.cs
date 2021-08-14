using Newtonsoft.Json;
using PushNotifications.Server.Google;

namespace PushNotifications.Server.Tests.Testdata
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

            internal static FcmResponse GetFcmResponse_Error_InvalidArgument()
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

            internal static FcmResponse GetFcmResponse_Error_NotFound()
            {
                var originalFcmResponseJson =
                "{\n" +
                "  \"error\": {\n" +
                "    \"code\": 404,\n" +
                "    \"message\": \"Requested entity was not found.\",\n" +
                "    \"status\": \"NOT_FOUND\",\n" +
                "    \"details\": [\n" +
                "      {\n" +
                "        \"@type\": \"type.googleapis.com/google.firebase.fcm.v1.FcmError\",\n" +
                "        \"errorCode\": \"UNREGISTERED\"\n" +
                "      }\n" +
                "    ]\n" +
                "  }\n" +
                "}";

                var fcmResponse = JsonConvert.DeserializeObject<FcmResponse>(originalFcmResponseJson);
                fcmResponse.Token = "token";
                return fcmResponse;
            }
        }
    }
}
