namespace PushNotifications.Server.AspNetCore
{
    internal interface IPushNotificationClientFactory
    {
        IPushNotificationClient GetClient();
    }
}