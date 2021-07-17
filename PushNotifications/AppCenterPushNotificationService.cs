using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using PushNotifications.Logging;
using PushNotifications.Messages;
using Newtonsoft.Json;

namespace PushNotifications
{
    public class AppCenterPushNotificationService : IPushNotificationService
    {
        private readonly ILogger logger;
        private readonly HttpClient httpClient;
        private readonly IAppCenterConfiguration appCenterConfiguration;
        private readonly JsonSerializerSettings jsonSerializerSettings;

        public AppCenterPushNotificationService(IAppCenterConfiguration appCenterConfiguration)
            : this(Logger.Current, new HttpClient(), appCenterConfiguration)
        {
        }

        public AppCenterPushNotificationService(ILogger logger, HttpClient httpClient, IAppCenterConfiguration appCenterConfiguration)
        {
            this.logger = logger;
            this.httpClient = httpClient;
            this.appCenterConfiguration = this.EnsureConfig(appCenterConfiguration);

            var apiToken = this.appCenterConfiguration.ApiToken;
            this.httpClient.DefaultRequestHeaders.TryAddWithoutValidation("X-API-Token", apiToken);

            this.jsonSerializerSettings = new JsonSerializerSettings
            {
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                DateParseHandling = DateParseHandling.DateTime,
                DateTimeZoneHandling = DateTimeZoneHandling.Utc
            };
        }

        private IAppCenterConfiguration EnsureConfig(IAppCenterConfiguration config)
        {
            if (string.IsNullOrEmpty(config.ApiToken))
            {
                this.logger.Log(LogLevel.Error, $"Invalid ApiToken");
                throw new ArgumentException($"Use ApiToken provided by AppCenter", nameof(config.ApiToken));
            }

            if (string.IsNullOrEmpty(config.OrganizationName))
            {
                this.logger.Log(LogLevel.Error, $"Invalid OrganizationName");
                throw new ArgumentException($"Use OrganizationName provided by AppCenter", nameof(config.OrganizationName));
            }

            if (config.AppNames == null || config.AppNames.Count == 0)
            {
                this.logger.Log(LogLevel.Error, $"Invalid AppNames");
                throw new ArgumentException($"Use AppNames set-up in AppCenter", nameof(config.AppNames));
            }

            return config;
        }

        /// <summary>
        ///     Sends <paramref name="appCenterPushMessage" /> to AppCenter API
        ///     /v0.1​/apps​/{owner_name}​/{app_name}​/push​/notifications
        /// </summary>
        /// <param name="appCenterPushMessage"></param>
        /// <returns>
        ///     Returns a <seealso cref="AppCenterPushResponse" /> for each target runtime platform.
        ///     This can either be <seealso cref="AppCenterPushSuccess" /> or <seealso cref="AppCenterPushError" />.
        /// </returns>
        public async Task<IEnumerable<AppCenterPushResponse>> SendPushNotificationAsync(AppCenterPushMessage appCenterPushMessage)
        {
            if (appCenterPushMessage == null)
            {
                throw new ArgumentNullException(nameof(appCenterPushMessage));
            }

            this.logger.Log(LogLevel.Debug, "SendPushNotificationAsync");
            var pushResponses = new List<AppCenterPushResponse>();

            var organizationName = this.appCenterConfiguration.OrganizationName;
            var appNames = this.appCenterConfiguration.AppNames;

            foreach (var appNameMappings in appNames)
            {
                AppCenterPushResponse appCenterPushResponse;
                try
                {
                    var appName = appNameMappings.Value;
                    var requestUri = $"https://appcenter.ms/api/v0.1/apps/{organizationName}/{appName}/push/notifications";
                    this.logger.Log(LogLevel.Debug, $"SendPushNotificationAsync with requestUri={requestUri}");

                    var serialized = JsonConvert.SerializeObject(appCenterPushMessage, this.jsonSerializerSettings);
                    var httpContent = new StringContent(serialized, Encoding.UTF8, "application/json");

                    var httpResponseMessage = await this.httpClient.PostAsync(requestUri, httpContent);

                    var jsonResponse = await httpResponseMessage.Content.ReadAsStringAsync();
                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        appCenterPushResponse = JsonConvert.DeserializeObject<AppCenterPushSuccess>(jsonResponse, this.jsonSerializerSettings);
                    }
                    else
                    {
                        appCenterPushResponse = JsonConvert.DeserializeObject<AppCenterPushError>(jsonResponse, this.jsonSerializerSettings);
                    }
                }
                catch (Exception ex)
                {
                    var errorMessage = $"Failed to send push notification request to app center: {ex.Message}";
                    this.logger.Log(LogLevel.Error, errorMessage);

                    appCenterPushResponse = new AppCenterPushError
                    {
                        ErrorMessage = errorMessage,
                        ErrorCode = "-1"
                    };
                }

                appCenterPushResponse.RuntimePlatform = appNameMappings.Key;
                pushResponses.Add(appCenterPushResponse);
            }

            return pushResponses;
        }

        /// <summary>
        ///     Gets a list of <seealso cref="AppCenterPushContent"/> from AppCenter API
        ///     /v0.1​/apps​/{owner_name}​/{app_name}​/push​/notifications
        /// </summary>
        /// <returns>
        ///     Returns a <seealso cref="AppCenterPushContent" /> for each target runtime platform.
        /// </returns>
        public async Task<IEnumerable<NotificationOverviewResult>> GetPushNotificationsAsync(int top = 30)
        {
            this.logger.Log(LogLevel.Debug, "GetPushNotificationsAsync");
            var notificationOverviewResults = new List<NotificationOverviewResult>();

            var organizationName = this.appCenterConfiguration.OrganizationName;
            var appNames = this.appCenterConfiguration.AppNames;

            foreach (var appNameMappings in appNames)
            {
                try
                {
                    var appName = appNameMappings.Value;
                    var queryParameters = $"%24top={top:D}&%24orderby=count%20desc&%24inlinecount=none";
                    var requestUri = $"https://appcenter.ms/api/v0.1/apps/{organizationName}/{appName}/push/notifications?{queryParameters}";
                    this.logger.Log(LogLevel.Debug, $"GetPushNotificationsAsync with requestUri={requestUri}");

                    var httpResponseMessage = await this.httpClient.GetAsync(requestUri);

                    var jsonResponse = await httpResponseMessage.Content.ReadAsStringAsync();
                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        var result = JsonConvert.DeserializeObject<NotificationOverviewResultInternal>(jsonResponse, this.jsonSerializerSettings);
                        foreach (var notificationOverviewResult in result.Values)
                        {
                            notificationOverviewResult.RuntimePlatform = appNameMappings.Key;
                        }
                        notificationOverviewResults.AddRange(result.Values);

                    }
                }
                catch (Exception ex)
                {
                    var errorMessage = $"GetPushNotificationsAsync failed with exception: {ex.Message}";
                    this.logger.Log(LogLevel.Error, errorMessage);

                    throw new Exception(errorMessage, ex);
                }
            }

            return notificationOverviewResults;
        }
    }
}