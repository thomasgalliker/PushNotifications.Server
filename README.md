# PushNotifications
[![Version](https://img.shields.io/nuget/v/PushNotifications.svg)](https://www.nuget.org/packages/PushNotifications)  [![Downloads](https://img.shields.io/nuget/dt/PushNotifications.svg)](https://www.nuget.org/packages/PushNotifications)

<img src="https://raw.githubusercontent.com/thomasgalliker/PushNotifications/develop/logo.png" height="100" alt="PushNotifications" align="right">
Server-side .NET SDK for Microsoft's AppCenter.Push.

### Download and Install PushNotifications
This library is available on NuGet: https://www.nuget.org/packages/PushNotifications/
Use the following command to install PushNotifications using NuGet package manager console:

    PM> Install-Package PushNotifications

You can use this library in any .NET project which is compatible to .NET Framework 4.5+ and .NET Standard 1.2+.

### API Usage
The following sections document basic use cases of this library.

#### Create Push Notifications
Creating a push notification starts off with an object called `AppCenterPushMessage`. This object has two basic properties: The `Content` of the push notification and to whom we want to send the notification (the `Target`).
There are several different Targets supported; all of them inherit from `AppCenterPushTarget`:
- `AppCenterPushAccountIdsTarget`
- `AppCenterPushAudiencesTarget`
- `AppCenterPushDevicesTarget`
- `AppCenterPushUserIdsTarget`

Following code illustrated an example `AppCenterPushMessage` which sends a message to three account IDs:
```C#
var appCenterPushMessage = new AppCenterPushMessage
{
    Content = new AppCenterPushContent
    {
        Name = $"AppCenterPushAccountIdsTarget_{Guid.NewGuid():B}",
        Title = "Push From App Center",
        Body = "Hello! Isn't this an amazing notification message?",
        CustomData = new Dictionary<string, string> { { "key", "value" } }
    },
    Target = new AppCenterPushAccountIdsTarget
    {
        AccountIds = new List<string>
        {
            "A1DF0327-3945-4B24-B22C-CC34367BEFE3",
            "DF2D5140-CF24-4921-9045-9FE963112981",
            "7A3E97D4-3BDA-4DFB-89CD-4C46AAEFF548",
        }
    }
};
```

#### Sending Push Notifications
Following steps are required to send a AppCenterPushMessage out to AppCenter:
- Create an implementation of IAppCenterConfiguration and provide ApiToken, OrganizationName as well as the platform-specific app names.
- Create an instance of AppCenterPushNotificationService and pass the IAppCenterConfiguration as constructor parameter. For even more comfort, use IoC and resolve IPushNotificationService.

```C#
var appCenterConfiguration = new TestAppCenterConfiguration();
var pushNotificationService = new AppCenterPushNotificationService(appCenterConfiguration);
```

- Finally, call SendPushNotificationAsync with your push notification message.
```C#
var responseDtos = await pushNotificationService.SendPushNotificationAsync(appCenterPushMessage);
```
-  The response is `IEnumerable<AppCenterPushResponse>` where `AppCenterPushResponse` can either be `AppCenterPushSuccess` or `AppCenterPushError`. Since separate channels are used to send push messages to the targets, the responses contain a RuntimePlatform property which indicates the particular platform (e.g. Android or iOS) for which the response is valid.

### Links
Sending Push Notifications
https://docs.microsoft.com/en-us/appcenter/push/send-notification

Using the Push API
https://docs.microsoft.com/en-us/appcenter/push/rest-api

### Contribution
Contributors welcome! If you find a bug or you want to propose a new feature, feel free to do so by opening a new issue on github.com.

### License
This project is Copyright &copy; 2021 [Thomas Galliker](https://ch.linkedin.com/in/thomasgalliker). Free for non-commercial use. For commercial use please contact the author.
