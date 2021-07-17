using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using PushNotifications.Messages;
using Newtonsoft.Json;

namespace PushNotifications.Tests.Testdata
{
    public static class HttpResponseMessages
    {
        public static class AppCenterPushResponses
        {
            public static HttpResponseMessage Success(string notificationId)
            {
                return new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonConvert.SerializeObject(new AppCenterPushSuccess
                    {
                        NotificationId = notificationId
                    }))
                };
            }

            public static HttpResponseMessage Unauthorized(string errorMessage, string errorCode)
            {
                return new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.Unauthorized,
                    Content = new StringContent(JsonConvert.SerializeObject(new AppCenterPushError
                    {
                        ErrorMessage = errorMessage,
                        ErrorCode = errorCode,
                    }))
                };
            }
        }

    }
}