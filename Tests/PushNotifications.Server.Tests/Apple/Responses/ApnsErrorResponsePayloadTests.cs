using System;
using FluentAssertions;
using Newtonsoft.Json;
using PushNotifications.Server.Apple;
using Xunit;

namespace PushNotifications.Server.Tests.Apple
{
    [Trait("Category", "UnitTests")]
    public class ApnsErrorResponsePayloadTests
    {
        [Theory]
        [ClassData(typeof(UnixTimestampMillisecondsJsonConverterTestdata))]
        public void ShouldDeSerialize(ApnsErrorResponsePayload input)
        {
            // Arrange
            var json = JsonConvert.SerializeObject(input);

            // Act
            var output = JsonConvert.DeserializeObject<ApnsErrorResponsePayload>(json);

            // Assert
            output.Should().NotBeNull();
            output.Reason.Should().Be(input.Reason);
            output.Timestamp.Should().Be(input.Timestamp);
        }

        public class UnixTimestampMillisecondsJsonConverterTestdata : TheoryData<ApnsErrorResponsePayload>
        {
            public UnixTimestampMillisecondsJsonConverterTestdata()
            {
                this.Add(new ApnsErrorResponsePayload { Reason = null, Timestamp = null });
                this.Add(new ApnsErrorResponsePayload { Reason = ApnsResponseReason.Unregistered, Timestamp = null });
                this.Add(new ApnsErrorResponsePayload { Reason = null, Timestamp = new DateTimeOffset(new DateTime(2000, 1, 1)) });
            }
        }
    }
}