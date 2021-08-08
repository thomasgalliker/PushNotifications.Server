# PushNotifications
[![Version](https://img.shields.io/nuget/v/PushNotifications.Server.svg)](https://www.nuget.org/packages/PushNotifications.Server)  [![Downloads](https://img.shields.io/nuget/dt/PushNotifications.Server.svg)](https://www.nuget.org/packages/PushNotifications.Server)

<img src="https://raw.githubusercontent.com/thomasgalliker/PushNotifications/develop/Images/logo.png" height="100" alt="PushNotifications" align="right">
Server-side .NET library for sending push notifications to Apple Push Notification Service (APNS) and Google's Firebase Cloud Messaging (FCM).

### Download and Install PushNotifications
This library is available on NuGet: https://www.nuget.org/packages/PushNotifications.Server
Use the following command to install PushNotifications.Server using NuGet package manager console:

    PM> Install-Package PushNotifications.Server

You can use this library in any .NET project which is compatible to .NET Standard 2.0 and higher.

### ASP.NET Core Integration
For a smooth integration with ASP.NET Core projects, use following NuGet package: https://www.nuget.org/packages/PushNotifications.Server.AspNetCore
Use the following command to install PushNotifications.Server.AspNetCore using NuGet package manager console:

    PM> Install-Package PushNotifications.Server.AspNetCore

You can use this library in any ASP.NET Core project which is compatible to .NET Core 3.1 and higher.

#### Configure using appsettings.json

### API Usage
The following sections document basic use cases of this library. The following code excerpts can also be found in the [sample applications](https://github.com/thomasgalliker/PushNotifications/tree/develop/Samples).

#### Cross-Platform Push Notifications
The goal of cross-platform push notification is to provide an abstracted request/response model in order to serve all unterlying platforms. `PushNotificationClient` is the implementation class of such a cross-platform push notification client. In order to create a new instance of PushNotificationClient, you have to create an instance of FcmClient and ApnsClient and pass it into PushNotificationClient.
```C#
var pushNotificationClient = new PushNotificationClient(fcmClient, apnsClient);
```
 
###### Sending PushRequests to all platforms
Cross-platform push requests are abstracted using class `PushRequest`. Create a new `PushRequest` and send it using the `SendAsync` method of `PushNotificationClient`.
```C#
var pushRequest = new PushRequest
{
    Content = new PushContent
    {
        Title = "Test Message",
        Body = $"Message @ {DateTime.Now}",
        CustomData = new Dictionary<string, string>
        {
            { "key", "value" }
        }
    },
    Devices = pushDevices
};

var pushResponse = await this.pushNotificationClient.SendAsync(pushRequest);
```

#### APNS Push Notifications (iOS / Apple)
Sending push notifications to iOS devices is pretty easy. Create a new instance of `ApnsClient`.
```C#
IApnsClient apnsClient = new ApnsClient(apnsJwtOptions);
```
Then, create an `ApnsRequest` with some title and body and send it out using `SendAsync` method.
```C#
var apnsRequest = new ApnsRequest(ApplePushType.Alert)
    .AddToken(token)
    .AddAlert("Test Message", $"Message @ {DateTime.Now}")
    .AddCustomProperty("key", "value");

var apnsResponse = await this.apnsClient.SendAsync(apnsRequest);
```

#### FCM Push Notifications (Android / Google)
In order to send FCM push notifications, you have to create a new instance of FcmClient. FcmClient requires an instance of FcmOptions, which contains the FCM configuration parameters which can be found on http://firebase.google.com.
You can either create a FcmOptions manually (`new FcmOptions{ ... }`) or by binding from a appsettings.json file. See sample projects for more info.

This library supports both, the old "legacy" FcmClient as well as the new "v1" FcmClient. Check the [firebase migration documentations](https://firebase.google.com/docs/cloud-messaging/migrate-v1) before before selecting one or the other.

###### Sending Push Notifications using FcmClient (HTTP v1 API)

```C#
IFcmClient fcmClient = new FcmClient(fcmOptions);
```
Create a new FcmRequest and send it using the SendAsync method of FcmClient.
```C#
var fcmRequest = new FcmRequest()
{
    Message = new Message
    {
        Token = token,
        Notification = new Notification
        {
            Title = "Test Message",
            Body = $"Message @ {DateTime.Now}",
        },
        Data = new Dictionary<string, string>
        {
            { "key", "value" }
        },
    },
    ValidateOnly = false,
};

var fcmResponse = await this.fcmClient.SendAsync(fcmRequest);
```

###### Sending Push Notifications using FcmClient (Legacy HTTP API)
All legacy FCM client related code can be found in namespace `PushNotifications.Server.Google.Legacy`. The way the legacy FcmClient works is similar to the v1 FcmClient. The main differences can be found in the `FcmOptions` as well as in the request/response model.
```C#
IFcmClient fcmClient = new FcmClient(fcmOptions);
```
Create a new FcmRequest and send it using the SendAsync method of FcmClient.
```C#
var fcmRequest = new FcmRequest()
{
    To = token,
    Notification = new FcmNotification
    {
        Title = "Test Message",
        Body = $"Message @ {DateTime.Now}",
    },
    Data = new Dictionary<string, string>
    {
        { "key", "value" }
    },
};

var fcmResponse = await this.fcmClient.SendAsync(fcmRequest);
```

### Links
Handling Notification Responses from APNs
https://developer.apple.com/documentation/usernotifications/setting_up_a_remote_notification_server/handling_notification_responses_from_apns

Firebase Cloud Messaging HTTP protocol
https://firebase.google.com/docs/cloud-messaging/http-server-ref

### Contribution
Contributors welcome! If you find a bug or you want to propose a new feature, feel free to do so by opening a new issue on github.com.

### License
This project is Copyright &copy; 2021 [Thomas Galliker](https://ch.linkedin.com/in/thomasgalliker). Free for non-commercial use. For commercial use please contact the author.
