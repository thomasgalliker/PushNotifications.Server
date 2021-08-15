using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Newtonsoft.Json;
using PushNotifications.Server.Google;
using PushNotifications.Server.Tests.Mocks;
using PushNotifications.Server.Tests.Testdata;
using PushNotifications.Server.Tests.Utils;
using Xunit;
using Xunit.Abstractions;

namespace PushNotifications.Server.Tests.Google.V1
{
    [Trait("Category", "UnitTests")]
    public class FcmClientTests
    {
        private readonly TestOutputHelperLogger<FcmClient> logger;
        private readonly HttpClientMock httpClientMock;

        public FcmClientTests(ITestOutputHelper testOutputHelper)
        {
            this.logger = new TestOutputHelperLogger<FcmClient>(testOutputHelper);
            this.httpClientMock = new HttpClientMock();
        }

        [Fact]
        public void ShouldThrowException_IfServiceAccountKeyFilDoesNotExist()
        {
            // Arrange
            var fcmOptions = new FcmOptions
            {
                ServiceAccountKeyFilePath = ".\\file-not-found.json"
            };

            // Act
            Action action = () => new FcmClient(this.logger, this.httpClientMock.Object, fcmOptions);

            // Assert
            action.Should().Throw<FileNotFoundException>();
        }

        [Fact]
        public async Task ShouldSendAsync_Succesful()
        {
            // Arrange
            this.httpClientMock.SetupSendAsync()
                .ReturnsAsync(HttpResponseMessages.Success(JsonConvert.SerializeObject(FcmResponses.V1.GetFcmResponse_Success())))
                .Verifiable();

            var fcmOptions = FcmTestOptions.V1.GetFcmOptions();

            var fcmClient = new FcmClient(this.logger, this.httpClientMock.Object, fcmOptions);

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
            fcmResponse.Name.Should().NotBeEmpty();
            fcmResponse.Token.Should().Be(token);
            fcmResponse.IsSuccessful.Should().BeTrue();

            this.httpClientMock.VerifySendAsync(
                request => request.Method == HttpMethod.Post &&
                           request.RequestUri == new Uri("https://fcm.googleapis.com/v1/projects/ch-superdev-fcmdemo/messages:send"),
                           Times.Exactly(1));

            this.httpClientMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldSendAsync_Failed()
        {
            // Arrange
            this.httpClientMock.SetupSendAsync()
                .ReturnsAsync(HttpResponseMessages.BadRequest(JsonConvert.SerializeObject(FcmResponses.V1.GetFcmResponse_Error_InvalidArgument())))
                .Verifiable();

            var fcmOptions = FcmTestOptions.V1.GetFcmOptions();

            var fcmClient = new FcmClient(this.logger, this.httpClientMock.Object, fcmOptions);

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
            fcmResponse.Name.Should().BeNull();
            fcmResponse.Token.Should().Be(token);
            fcmResponse.IsSuccessful.Should().BeFalse();

            this.httpClientMock.VerifySendAsync(
                request => request.Method == HttpMethod.Post &&
                           request.RequestUri == new Uri("https://fcm.googleapis.com/v1/projects/ch-superdev-fcmdemo/messages:send"),
                           Times.Exactly(1));

            this.httpClientMock.VerifyNoOtherCalls();
        }
    }
}