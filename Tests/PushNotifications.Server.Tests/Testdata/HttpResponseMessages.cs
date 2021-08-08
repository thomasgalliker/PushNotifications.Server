using System.Net;
using System.Net.Http;
using PushNotifications.Server.Internals;

namespace PushNotifications.Server.Tests.Testdata
{
    public static class HttpResponseMessages
    {
        public static HttpResponseMessage Success(string content = "")
        {
            return new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new JsonContent(content),
            };
        }

        public static HttpResponseMessage BadRequest(string content = "")
        {
            return new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.BadRequest,
                Content = new StringContent(content),
            };
        }
    }
}