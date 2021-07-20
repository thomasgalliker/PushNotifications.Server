using System;
using System.Collections.Generic;
using System.Dynamic;
using PushNotifications.Abstractions;

namespace PushNotifications.Apple
{
    public class ApnsRequest : IPushRequest
    {
        public string Token { get; private set; }

        public string VoipToken { get; private set; }

        public int Priority => this.CustomPriority ?? (this.Type == ApplePushType.Background ? 5 : 10); // 5 for background, 10 for everything else

        public ApplePushType Type { get; }

        /// <summary>
        /// If specified, this value will be used as a `apns-
        /// </summary>
        public int? CustomPriority { get; private set; }

        public ApplePushAlert Alert { get; private set; }
        
        public ApplePushLocalizedAlert LocalizedAlert { get; private set; }

        public int? Badge { get; private set; }
        
        public string Sound { get; private set; }

        /// <summary>
        /// See <a href="https://developer.apple.com/documentation/usernotifications/unnotificationcontent/1649866-categoryidentifier">official documentation</a> for reference.
        /// </summary>
        
        public string Category { get; private set; }

        public bool IsContentAvailable { get; private set; }
        
        public bool IsMutableContent { get; private set; }

        /// <summary>
        /// The date at which the notification is no longer valid.
        /// If set to <i>null</i> (default) then <i>apns-expiration</i> header is not sent and expiration time is undefined (<seealso href="https://stackoverflow.com/questions/44630196/what-is-the-default-value-of-the-apns-expiration-field">but is probably large</seealso>).
        /// </summary>
        public DateTimeOffset? Expiration { get; private set; }

        /// <summary>
        /// An identifier you use to coalesce multiple notifications into a single notification for the user. 
        /// Typically, each notification request causes a new notification to be displayed on the user’s device. 
        /// When sending the same notification more than once, use the same value in this header to coalesce the requests.
        /// <b>The value of this key must not exceed 64 bytes.</b>
        /// </summary>
        
        public string CollapseId { get; private set; }

        /// <summary>
        /// User-defined properties that will be attached to the root payload dictionary.
        /// </summary>
        public Dictionary<string, object> CustomProperties { get; set; }

        /// <summary>
        /// User-defined properties that will be attached to the <i>aps</i> payload dictionary.
        /// </summary>
        public IDictionary<string, object> CustomApsProperties { get; set; }

        /// <summary>
        /// Indicates whether alert must be sent as a string. 
        /// </summary>
        bool sendAlertAsText;

        public ApnsRequest(ApplePushType pushType)
        {
            this.Type = pushType;
        }

        /// <summary>
        /// Add `content-available: 1` to the payload.
        /// </summary>
        public ApnsRequest AddContentAvailable()
        {
            this.IsContentAvailable = true;
            return this;
        }

        /// <summary>
        /// Add `mutable-content: 1` to the payload.
        /// </summary>
        /// <returns></returns>
        public ApnsRequest AddMutableContent()
        {
            this.IsMutableContent = true;
            return this;
        }

        /// <summary>
        /// Add alert to the payload.
        /// </summary>
        /// <param name="title">Alert title. Can be null.</param>
        /// <param name="subtitle">Alert subtitle. Can be null.</param>
        /// <param name="body">Alert body. <b>Cannot be null.</b></param>
        /// <returns></returns>
        public ApnsRequest AddAlert( string title,  string subtitle, string body)
        {
            this.Alert = new ApplePushAlert(title, subtitle, body);
            if (title == null)
            {
                this.sendAlertAsText = true;
            }

            return this;
        }

        /// <summary>
        /// Add alert to the payload.
        /// </summary>
        /// <param name="title">Alert title. Can be null.</param>
        /// <param name="body">Alert body. <b>Cannot be null.</b></param>
        /// <returns></returns>
        public ApnsRequest AddAlert( string title, string body)
        {
            this.Alert = new ApplePushAlert(title, body);
            if (title == null)
            {
                this.sendAlertAsText = true;
            }

            return this;
        }

        /// <summary>
        /// Add alert to the payload.
        /// </summary>
        /// <param name="body">Alert body. <b>Cannot be null.</b></param>
        /// <returns></returns>
        public ApnsRequest AddAlert(string body)
        {
            return this.AddAlert(null, body);
        }

        /// <summary>
        /// Add localized alert to the payload. When alert is already present, localized alert will be omitted when generating payload.
        /// </summary>
        /// <param name="locKey">Key to an alert-message string in a Localizable.strings file for the current localization. <b>Cannot be null.</b></param>
        /// <param name="locArgs">Variable string values to appear in place of the format specifiers in loc-key. <b>Cannot be null.</b></param>
        /// <param name="titleLocKey">The key to a title string in the Localizable.strings file for the current localization. Can be null.</param>
        /// <param name="tittleLocArgs">Variable string values to appear in place of the format specifiers in title-loc-key. Can be null.</param>
        /// <param name="actionLocKey">The string is used as a key to get a localized string in the current localization to use for the right button’s title instead of “View". Can be null.</param>
        /// <returns></returns>
        public ApnsRequest AddLocalizedAlert( string titleLocKey,  string[] tittleLocArgs, string locKey, string[] locArgs,  string actionLocKey)
        {
            this.LocalizedAlert = new ApplePushLocalizedAlert(titleLocKey, tittleLocArgs, locKey, locArgs, actionLocKey);
            return this;
        }
        
        /// <summary>
        /// Add localized alert to the payload. When alert is already present, localized alert will be omitted when generating payload.
        /// </summary>
        /// <param name="locKey">Key to an alert-message string in a Localizable.strings file for the current localization. <b>Cannot be null.</b></param>
        /// <param name="locArgs">Variable string values to appear in place of the format specifiers in loc-key. <b>Cannot be null.</b></param>
        /// <returns></returns>
        public ApnsRequest AddLocalizedAlert(string locKey, string[] locArgs)
        {
            return this.AddLocalizedAlert(null, null, locKey, locArgs, null);
        }

        public ApnsRequest SetPriority(int priority)
        {
            if(priority < 0 || priority > 10)
            {
                throw new ArgumentOutOfRangeException(nameof(priority), priority, "Priority must be between 0 and 10.");
            }

            this.CustomPriority = priority;
            return this;
        }

        public ApnsRequest AddBadge(int badge)
        {
            this.IsContentAvailableGuard();
            if (this.Badge != null)
            {
                throw new InvalidOperationException("Badge already exists");
            }

            this.Badge = badge;
            return this;
        }

        public ApnsRequest AddSound(string sound = "default")
        {
            if (string.IsNullOrWhiteSpace(sound))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(sound));
            }

            this.IsContentAvailableGuard();
            if (this.Sound != null)
            {
                throw new InvalidOperationException("Sound already exists");
            }

            this.Sound = sound;
            return this;
        }

        public ApnsRequest AddCategory(string category)
        {
            if (string.IsNullOrWhiteSpace(category))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(category));
            }

            if (this.Category != null)
            {
                throw new InvalidOperationException($"{nameof(this.Category)} already exists.");
            }

            this.Category = category;
            return this;
        }

        /// <summary>
        ///  APNs stores the notification and tries to deliver it at least once, repeating the attempt as needed until the specified date.
        /// </summary>
        /// <param name="expirationDate">The date at which the notification is no longer valid.</param>
        /// 
        public ApnsRequest AddExpiration(DateTimeOffset expirationDate)
        {
            this.Expiration = expirationDate;
            return this;
        }

        /// <summary>
        /// APNs attempts to deliver the notification only once and doesn’t store it.
        /// </summary>
        /// <seealso cref="AddExpiration"/>
        public ApnsRequest AddImmediateExpiration()
        {
            this.Expiration = DateTimeOffset.MinValue;
            return this;
        }

        public ApnsRequest AddToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(token));
            this.EnsureTokensNotExistGuard();
            if (this.Type == ApplePushType.Voip)
                throw new InvalidOperationException($"Please use AddVoipToken() when sending {nameof(ApplePushType.Voip)} pushes.");
            this.Token = token;
            return this;
        }

        public ApnsRequest AddVoipToken(string voipToken)
        {
            if (string.IsNullOrWhiteSpace(voipToken))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(voipToken));
            this.EnsureTokensNotExistGuard();
            if(this.Type != ApplePushType.Voip)
                throw new InvalidOperationException($"VoIP token may only be used with {nameof(ApplePushType.Voip)} pushes.");
            this.VoipToken = voipToken;
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="addToApsDict">If <b>true</b>, property will be added to the <i>aps</i> dictionary, otherwise to the root dictionary. Default: <b>false</b>.</param>
        /// <returns></returns>
        public ApnsRequest AddCustomProperty(string key, object value, bool addToApsDict = false)
        {
            if (addToApsDict)
            {
                this.CustomApsProperties ??= new Dictionary<string, object>();
                this.CustomApsProperties.Add(key, value);
            }
            else
            {
                this.CustomProperties ??= new Dictionary<string, object>();
                this.CustomProperties.Add(key, value);
            }
            return this;
        }

        public ApnsRequest AddCollapseId(string collapseId)
        {
            if (string.IsNullOrEmpty(collapseId))
                throw new ArgumentException($"'{nameof(collapseId)}' cannot be null or empty", nameof(collapseId));
            if (!string.IsNullOrEmpty(this.CollapseId))
                throw new InvalidOperationException($"{nameof(this.CollapseId)} is already added.");
            this.CollapseId = collapseId;
            return this;
        }

        void EnsureTokensNotExistGuard()
        {
            if (!(string.IsNullOrEmpty(this.Token) && string.IsNullOrEmpty(this.VoipToken)))
                throw new InvalidOperationException("Notification already has token");
        }

        void IsContentAvailableGuard()
        {
            if (this.IsContentAvailable)
                throw new InvalidOperationException("Cannot add fields to a push with content-available");
        }

        public object GeneratePayload()
        {
            dynamic payload = new ExpandoObject();
            payload.aps = new ExpandoObject();
            IDictionary<string, object> apsAsDict = payload.aps;

            if (this.IsContentAvailable)
            {
                apsAsDict["content-available"] = "1";
            }

            if (this.IsMutableContent)
            {
                apsAsDict["mutable-content"] = "1";
            }

            if (this.Alert is ApplePushAlert applePushAlert)
            {
                object alert;
                if (this.sendAlertAsText)
                {
                    alert = applePushAlert.Body;
                }
                else if (applePushAlert.Subtitle == null)
                {
                    alert = new { title = applePushAlert.Title, body = applePushAlert.Body };
                }
                else
                {
                    alert = new { title = applePushAlert.Title, subtitle = applePushAlert.Subtitle, body = applePushAlert.Body };
                }

                payload.aps.alert = alert;
            }
            else if (this.LocalizedAlert != null)
            {
                object localizedAlert = this.LocalizedAlert;
                payload.aps.alert = localizedAlert;
            }

            if (this.Badge != null)
            {
                payload.aps.badge = this.Badge.Value;
            }

            if (this.Sound != null)
            {
                payload.aps.sound = this.Sound;
            }

            if (this.Category != null)
            {
                payload.aps.category = this.Category;
            }

            if (this.CustomProperties is IDictionary<string, object> customProperties)
            {
                IDictionary<string, object> payloadAsDict = payload;
                foreach (var customProperty in customProperties)
                {
                    payloadAsDict[customProperty.Key] = customProperty.Value;
                }
            }

            if (this.CustomApsProperties is IDictionary<string, object> customApsProperties)
            {
                foreach (var customApsProperty in customApsProperties)
                {
                    apsAsDict[customApsProperty.Key] = customApsProperty.Value;
                }
            }

            return payload;
        }
    }
}
