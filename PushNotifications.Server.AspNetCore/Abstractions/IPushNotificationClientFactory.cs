namespace PushNotifications.Server.Server.AspNetCore
{
    internal interface IPushNotificationClientFactory
    {
        IPushNotificationClient GetClient();
    }
}