using System;
using System.Collections.Generic;
using FluentAssertions;
using Newtonsoft.Json;
using PushNotifications.Google;
using Xunit;

namespace PushNotifications.Tests.Apple
{
    [Trait("Category", "UnitTests")]
    public class FcmRequestTests
    {
        [Fact]
        public void ShouldDeSerialize()
        {
            // Arrange
            var fcmRequest = new FcmRequest
            {
                Message = new Message
                {
                    Notification = new Notification
                    {
                        Title = "title",
                        Body = "body"
                    },
                    AndroidConfig = new AndroidConfig()
                    {
                        CollapseKey = "collapse_key",
                        Data = new Dictionary<string, string>()
                        {
                            { "key", "value" }
                        },
                        Priority = AndroidMessagePriority.High,
                        Notification = new AndroidNotification()
                        {
                            BodyLocArgs = new[] { "1", "2" },
                            Body = "body",
                            Color = "color",
                            Tag = "tag",
                            BodyLocKey = "body_loc_key",
                            ClickAction = "click_action",
                            Sound = "sound",
                            Icon = "icon",
                            Title = "title",
                            TitleLocArgs = new[] { "3", "4" },
                            TitleLocKey = "title_loc_key"
                        },
                        TimeToLive = TimeSpan.FromSeconds(10),
                        RestrictedPackageName = "restricted_package_name"
                    },
                    ApnsConfig = new ApnsConfig()
                    {
                        Payload = new ApnsConfigPayload()
                        {
                            Aps = new Aps()
                            {
                                Badge = 1,
                                Alert = new ApsAlert()
                                {
                                    TitleLocKey = "title_loc_key",
                                    ActionLocKey = "action_loc_key",
                                    TitleLocArgs = new[] { "1", "2" },
                                    Title = "Title",
                                    Body = "Body",
                                    LaunchImage = "LaunchImage",
                                    LocArgs = new[] { "3", "4" },
                                    LocKey = "LocKey"
                                },
                                Category = "category",
                                Sound = "sound",
                                CustomData = new Dictionary<string, object>()
                                {
                                    { "key", "value" }
                                },
                                ContentAvailable = true,
                                MutableContent = true,
                                ThreadId = "1"
                            },
                            CustomData = new Dictionary<string, object>()
                            {
                                { "key", "value" }
                            }
                        }
                    },
                    Token = "token",
                    Data = new Dictionary<string, string>()
                    {
                        { "key", "value" }
                    },
                }
            };

            // Act
            var json = JsonConvert.SerializeObject(fcmRequest);
            var output = JsonConvert.DeserializeObject<FcmRequest>(json);

            // Assert
            output.Should().BeEquivalentTo(fcmRequest);
        }
    }
}