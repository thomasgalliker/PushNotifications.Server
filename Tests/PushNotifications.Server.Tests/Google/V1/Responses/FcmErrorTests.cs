using FluentAssertions;
using Newtonsoft.Json;
using PushNotifications.Server.Google;
using Xunit;

namespace PushNotifications.Server.Tests.Google
{
    [Trait("Category", "UnitTests")]
    public class FcmErrorTests
    {
        [Theory]
        [ClassData(typeof(FcmErrorTestdata))]
        public void ShouldDeSerialize(FcmError input)
        {
            // Arrange
            var json = JsonConvert.SerializeObject(input);

            // Act
            var output = JsonConvert.DeserializeObject<FcmError>(json);

            // Assert
            output.Should().NotBeNull();
            output.Code.Should().Be(input.Code);
            output.Message.Should().Be(input.Message);
            output.Status.Should().Be(input.Status);
        }

        public class FcmErrorTestdata : TheoryData<FcmError>
        {
            public FcmErrorTestdata()
            {
                this.Add(new FcmError
                {
                    Code = 0,
                    Message = null,
                    Status = null
                });
                this.Add(new FcmError
                {
                    Code = 400,
                    Message = "message",
                    Status = FcmErrorCode.InvalidArgument
                });
            }
        }
    }
}