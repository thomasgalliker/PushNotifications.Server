using FluentAssertions;
using Newtonsoft.Json;
using PushNotifications.Google;
using Xunit;

namespace PushNotifications.Tests.Google
{
    [Trait("Category", "UnitTests")]
    public class AndroidConfigTests
    {
        [Theory]
        [ClassData(typeof(AndroidConfigTestdata))]
        public void ShouldDeSerialize(AndroidConfig input)
        {
            // Arrange
            var json = JsonConvert.SerializeObject(input);

            // Act
            var output = JsonConvert.DeserializeObject<AndroidConfig>(json);

            // Assert
            output.Should().NotBeNull();
            output.Priority.Should().Be(input.Priority);
        }

        public class AndroidConfigTestdata : TheoryData<AndroidConfig>
        {
            public AndroidConfigTestdata()
            {
                this.Add(new AndroidConfig { Priority = AndroidMessagePriority.Normal });
                this.Add(new AndroidConfig { Priority = AndroidMessagePriority.High });
            }
        }
    }
}