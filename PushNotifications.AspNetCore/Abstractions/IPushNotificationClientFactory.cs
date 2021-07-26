namespace PushNotifications.AspNetCore
{
    internal interface IPushNotificationClientFactory
    {
        IPushNotificationClient GetClient();
    }
}