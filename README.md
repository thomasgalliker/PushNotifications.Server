# PushNotifications
[![Version](https://img.shields.io/nuget/v/PushNotifications.NET.svg)](https://www.nuget.org/packages/PushNotifications.NET)  [![Downloads](https://img.shields.io/nuget/dt/PushNotifications.NET.svg)](https://www.nuget.org/packages/PushNotifications.NET)

<img src="https://raw.githubusercontent.com/thomasgalliker/PushNotifications/develop/Images/logo.png" height="100" alt="PushNotifications" align="right">
Server-side .NET library for sending push notifications to Apple Push Notification Service (APNS) and Google's Firebase Cloud Messaging (FCM).

### Download and Install PushNotifications
This library is available on NuGet: https://www.nuget.org/packages/PushNotifications.NET
Use the following command to install PushNotifications.NET using NuGet package manager console:

    PM> Install-Package PushNotifications.NET

You can use this library in any .NET project which is compatible to .NET Standard 2.0 and higher.

### ASP.NET Core Integration
For a smooth integration with ASP.NET Core projects, use following NuGet package: https://www.nuget.org/packages/PushNotifications.AspNetCore
Use the following command to install PushNotifications.AspNetCore using NuGet package manager console:

    PM> Install-Package PushNotifications.AspNetCore

You can use this library in any ASP.NET Core project which is compatible to .NET Core 3.1 and higher.


### API Usage
The following sections document basic use cases of this library. The following code excerpts can also be found in the [sample applications](https://github.com/thomasgalliker/PushNotifications/tree/develop/Samples).

#### Sending Cross-Platform Push Notifications
tbd
```C#
var pushRequest = new PushRequest
{
    Content = new PushContent
    {
        Title = "Test Message",
        Body = $"Message from PushNotifications.AspNetCoreSample @ {DateTime.Now}",
        CustomData = new Dictionary<string, string>
        {
            { "key", "value" }
        }
    },
    Devices = pushDevices
};

var pushResponse = await this.pushNotificationClient.SendAsync(pushRequest);
```


#### Sending Push Notifications to FCM (Android)
tbd
```C#
var fcmRequest = new FcmRequest()
{
    To = token,
    Notification = new FcmNotification
    {
        Title = "Test Message",
        Body = $"Message from PushNotifications.AspNetCoreSample @ {DateTime.Now}",
    },
    Data = new Dictionary<string, string>
    {
        { "key", "value" }
    },
};

var fcmResponse = await this.fcmClient.SendAsync(fcmRequest);
```

#### Sending Push Notifications to APNS (iOS)
tbd
```C#
var apnsRequest = new ApnsRequest(ApplePushType.Alert)
    .AddToken(token)
    .AddAlert("Test Message", $"Message from PushNotifications.AspNetCoreSample @ {DateTime.Now}")
    .AddCustomProperty("key", "value");

var apnsResponse = await this.apnsClient.SendAsync(apnsRequest);
```

### Links
Sending Push Notifications
https://docs.microsoft.com/en-us/appcenter/push/send-notification

Using the Push API
https://docs.microsoft.com/en-us/appcenter/push/rest-api

### Contribution
Contributors welcome! If you find a bug or you want to propose a new feature, feel free to do so by opening a new issue on github.com.

### License
This project is Copyright &copy; 2021 [Thomas Galliker](https://ch.linkedin.com/in/thomasgalliker). Free for non-commercial use. For commercial use please contact the author.
