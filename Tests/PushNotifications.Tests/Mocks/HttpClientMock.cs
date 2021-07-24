using System;
using System.Linq.Expressions;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.Language.Flow;
using Moq.Protected;

namespace PushNotifications.Tests.Mocks
{
    /// <summary>
    /// Helps mocking HttpClients.
    ///
    /// Source: https://gingter.org/2018/07/26/how-to-mock-httpclient-in-your-net-c-unit-tests/
    /// </summary>
    public class HttpClientMock
    {
        private readonly Mock<HttpMessageHandler> httpMessageHandlerMock;

        public HttpClient Object { get; }

        public HttpClientMock()
        {
            this.httpMessageHandlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            this.Object = new HttpClient(this.httpMessageHandlerMock.Object);
        }

        public ISetup<HttpMessageHandler, Task<HttpResponseMessage>> SetupSendAsync()
        {
            return this.httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                );
        }

        public void VerifySendAsync(Expression<Func<HttpRequestMessage, bool>> match, Times times)
        {
            this.httpMessageHandlerMock.Protected().Verify(
                "SendAsync",
                times,
                ItExpr.Is(match),
                ItExpr.IsAny<CancellationToken>()
            );
        }

        public void VerifyNoOtherCalls()
        {
            this.httpMessageHandlerMock.VerifyNoOtherCalls();
        }
    }
}