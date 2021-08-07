using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Newtonsoft.Json;
using PushNotifications.Google;
using PushNotifications.Tests.Mocks;
using PushNotifications.Tests.Testdata;
using PushNotifications.Tests.Utils;
using Xunit;
using Xunit.Abstractions;

namespace PushNotifications.Tests.Google.V1
{
    [Trait("Category", "UnitTests")]
    public class FcmClientTests
    {
        private readonly TestOutputHelperLogger<FcmClient> logger;

        public FcmClientTests(ITestOutputHelper testOutputHelper)
        {
            this.logger = new TestOutputHelperLogger<FcmClient>(testOutputHelper);
        }

        [Fact]
        public async Task ShouldSendAsync_Succesful()
        {
            // Arrange
            var httpClientMock = new HttpClientMock();
            httpClientMock.SetupSendAsync()
                .ReturnsAsync(HttpResponseMessages.Success(JsonConvert.SerializeObject(FcmResponses.GetFcmResponse_Success())))
                .Verifiable();

            var fcmOptions = FcmTestOptions.V1.GetFcmOptions();

            var fcmClient = new FcmClient(this.logger, httpClientMock.Object, fcmOptions);

            var token = new string('A', 152);
            var fcmRequest = new FcmRequest()
            {
                Message = new Message
                {
                    Token = token,
                    Notification = new Notification
                    {
                        Title = "title",
                        Body = "body",
                    },
                    Data = new Dictionary<string, string>
                    {
                        { "key", "value" }
                    },
                },
                ValidateOnly = false,
            };

            // Act
            var fcmResponse = await fcmClient.SendAsync(fcmRequest);

            // Assert
            fcmResponse.Should().NotBeNull();
            fcmResponse.Token.Should().Be(token);

            httpClientMock.VerifySendAsync(
                request => request.Method == HttpMethod.Post &&
                           request.RequestUri == new Uri("https://fcm.googleapis.com/v1/projects/ch-superdev-fcmdemo/messages:send"),
                           Times.Exactly(1));

            httpClientMock.VerifyNoOtherCalls();
        }
    }
}