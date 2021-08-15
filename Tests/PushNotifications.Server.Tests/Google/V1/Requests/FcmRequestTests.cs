using System;
using System.Collections.Generic;
using FluentAssertions;
using Newtonsoft.Json;
using PushNotifications.Server.Google;
using Xunit;

namespace PushNotifications.Server.Tests.Apple
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
                            TitleLocKey = "title_loc_key",
                            ChannelId = "channelId",
                            Sticky = true,
                            EventTime = new DateTime(2000, 1, 1, 23, 59, 59, DateTimeKind.Utc),
                            LocalOnly = false,
                            NotificationPriority = NotificationPriority.High,
                            DefaultSound = false,
                            DefaultLightSettings = false,
                            DefaultVibrateTimings = false,
                            //VibrateTimings = new[] { TimeSpan.FromSeconds(1), TimeSpan.FromMilliseconds(250), TimeSpan.FromSeconds(1.2d) },
                            Visibility = Visibility.Unspecified,
                            NotificationCount = 99,
                            LightSettings = new LightSettings
                            {
                                Color = new Color
                                {
                                    Red = 255,
                                    Green = 255,
                                    Blue = 255,
                                    Alpha = 0,
                                },
                                LightOnDuration = TimeSpan.FromMilliseconds(1),
                                LightOffDuration = TimeSpan.FromMilliseconds(500),
                            },
                            Image = "ic_launcher"
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