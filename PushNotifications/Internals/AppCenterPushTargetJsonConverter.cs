using System;
using PushNotifications.Messages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PushNotifications
{
    public class AppCenterPushTargetJsonConverter : AbstractJsonConverter<AppCenterPushTarget>
    {
        protected override AppCenterPushTarget Create(Type objectType, JObject jObject)
        {
            if (jObject.TryGetValue("type", StringComparison.CurrentCultureIgnoreCase, out var jToken))
            {
                var typeString = (string)jToken;
                if (typeString == "account_ids_target")
                {
                    return new AppCenterPushAccountIdsTarget();
                }

                if (typeString == "audiences_target")
                {
                    return new AppCenterPushAudiencesTarget();
                }

                if (typeString == "devices_target")
                {
                    return new AppCenterPushDevicesTarget();
                }

                if (typeString == "user_ids_target")
                {
                    return new AppCenterPushUserIdsTarget();
                }
            }

            throw new NotSupportedException($"Conversion of {nameof(AppCenterPushTarget)} not supported.");
        }
    }
}