﻿using PushNotifications.Server.Google;

namespace PushNotifications.Server.AspNetCore.Google
{
    internal interface IFcmClientFactory
    {
        IFcmClient GetClient();

        bool TryGet(out IFcmClient fcmClient);
    }
}
