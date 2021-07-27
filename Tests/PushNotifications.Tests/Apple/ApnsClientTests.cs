using System;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Newtonsoft.Json;
using PushNotifications.Apple;
using PushNotifications.Tests.Mocks;
using PushNotifications.Tests.Testdata;
using PushNotifications.Tests.Utils;
using Xunit;
using Xunit.Abstractions;

namespace PushNotifications.Tests.Apple
{
    [Trait("Category", "UnitTests")]
    public class ApnsClientTests
    {
        private readonly TestOutputHelperLogger<ApnsClient> logger;

        public ApnsClientTests(ITestOutputHelper testOutputHelper)
        {
            this.logger = new TestOutputHelperLogger<ApnsClient>(testOutputHelper);
        }

        [Fact]
        public async Task ShouldSendAsync_Succesful()
        {
            // Arrange
            var httpClientMock = new HttpClientMock();
            httpClientMock.SetupSendAsync()
                .ReturnsAsync(HttpResponseMessages.Success())
                .Verifiable();

            var apnsJwtOptions = TestConfigurations.GetApnsJwtOptions();

            var apnsClient = new ApnsClient(this.logger, httpClientMock.Object, apnsJwtOptions);

            var token = new string('X', 64);
            var apnsRequest = new ApnsRequest(ApplePushType.Alert)
                .AddToken(token)
                .AddAlert("title", "body")
                .AddCustomProperty("key", "value");

            // Act
            var apnsResponse = await apnsClient.SendAsync(apnsRequest);

            // Assert
            apnsResponse.Should().NotBeNull();
            apnsResponse.Reason.Should().BeNull();
            apnsResponse.Token.Should().Be(token);

            httpClientMock.VerifySendAsync(
                request => request.Method == HttpMethod.Post &&
                           request.RequestUri == new Uri("https://api.development.push.apple.com/3/device/XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX"),
                           Times.Exactly(1));

            httpClientMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldSendAsync_Failed()
        {
            // Arrange
            var httpClientMock = new HttpClientMock();
            httpClientMock.SetupSendAsync()
                .ReturnsAsync(HttpResponseMessages.BadRequest(JsonConvert.SerializeObject(ApnsErrorResponsePayloads.GetApnsErrorResponsePayload())))
                .Verifiable();

            var apnsJwtOptions = TestConfigurations.GetApnsJwtOptions();

            var apnsClient = new ApnsClient(this.logger, httpClientMock.Object, apnsJwtOptions);

            var token = new string('X', 64);
            var apnsRequest = new ApnsRequest(ApplePushType.Alert)
                .AddToken(token)
                .AddAlert("title", "body")
                .AddCustomProperty("key", "value");

            // Act
            var apnsResponse = await apnsClient.SendAsync(apnsRequest);

            // Assert
            apnsResponse.Should().NotBeNull();
            apnsResponse.Reason.Should().Be(ApnsResponseReason.Unregistered);
            apnsResponse.Token.Should().Be(token);

            httpClientMock.VerifySendAsync(
                request => request.Method == HttpMethod.Post &&
                           request.RequestUri == new Uri("https://api.development.push.apple.com/3/device/XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX"),
                           Times.Exactly(1));

            httpClientMock.VerifyNoOtherCalls();
        }
    }
}