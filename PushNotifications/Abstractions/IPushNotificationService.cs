using System.Collections.Generic;
using System.Threading.Tasks;
using PushNotifications.Messages;

namespace PushNotifications
{
    public interface IPushNotificationService
    {
        Task<IEnumerable<AppCenterPushResponse>> SendPushNotificationAsync(AppCenterPushMessage appCenterPushMessage);

        Task<IEnumerable<NotificationOverviewResult>> GetPushNotificationsAsync(int top = 30);
    }
}