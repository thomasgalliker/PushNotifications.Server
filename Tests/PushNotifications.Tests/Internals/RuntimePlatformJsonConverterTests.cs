using FluentAssertions;
using Newtonsoft.Json;
using Xunit;

namespace PushNotifications.Tests.Internals
{
    [Trait("Category", "UnitTests")]
    public class RuntimePlatformJsonConverterTests
    {
        [Fact]
        public void ShouldConvertJsonToRuntimePlatform_Null()
        {
            // Arrange
            RuntimePlatform runtimePlatform = (string)null;

            var converter = new RuntimePlatformJsonConverter();
            var json = JsonConvert.SerializeObject(runtimePlatform, converter);

            // Act
            var deserializedRuntimePlatform = JsonConvert.DeserializeObject<RuntimePlatform>(json, converter);

            // Assert
            deserializedRuntimePlatform.Should().BeNull();
        }

        [Theory]
        [ClassData(typeof(RuntimePlatformJsonConverterTestdata))]
        public void ShouldConvertJsonToRuntimePlatform_Valid(RuntimePlatform runtimePlatform)
        {
            // Arrange
            var converter = new RuntimePlatformJsonConverter();
            var json = JsonConvert.SerializeObject(runtimePlatform, converter);

            // Act
            var deserializedRuntimePlatform = JsonConvert.DeserializeObject<RuntimePlatform>(json, converter);

            // Assert
            deserializedRuntimePlatform.Should().NotBeNull();
            deserializedRuntimePlatform.Should().BeOfType(runtimePlatform.GetType());
        }

        public class RuntimePlatformJsonConverterTestdata : TheoryData<RuntimePlatform>
        {
            public RuntimePlatformJsonConverterTestdata()
            {
                this.Add(new RuntimePlatform(""));
                this.Add(new RuntimePlatform("android"));
                this.Add(new RuntimePlatform("ios"));
                this.Add(new RuntimePlatform("uwp"));
            }
        }
    }
}