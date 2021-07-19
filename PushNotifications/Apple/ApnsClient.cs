using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PushNotifications.Abstractions;
using PushNotifications.Internals;
using PushNotifications.Logging;

namespace PushNotifications.Apple
{
    public partial class ApnsClient : IApnsClient
    {
        internal const string DevelopmentEndpoint = "https://api.development.push.apple.com";   // --> Sandbox
        internal const string ProductionEndpoint = "https://api.push.apple.com";                // --> Production
        private const string bundleIdVoipSuffix = ".voip";

        private readonly HttpClient httpClient;
        private readonly CngKey key;
        private readonly string keyId;
        private readonly string teamId;
        private readonly string bundleId;
        private readonly bool useCert;
        private readonly bool isVoipCert;

        private string jwt;
        private DateTime lastJwtGenerationTime;
        private readonly object jwtRefreshLock = new object();

        private bool useSandbox;
        private bool useBackupPort;

        /// <summary>
        /// Constructs a client instance with given <paramref name="options"/>
        /// for token-based authentication (using a .p8 certificate).
        /// </summary>
        private ApnsClient(HttpClient httpClient)
        {
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));

            this.httpClient.DefaultRequestHeaders.UserAgent.Clear();
            this.httpClient.DefaultRequestHeaders.UserAgent.Add(HttpClientUtils.GetProductInfo(this));
        }

        public ApnsClient(HttpClient httpClient, ApnsJwtOptions options) : this(httpClient)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            string certContent;
            if (options.CertFilePath != null)
            {
                certContent = File.ReadAllText(options.CertFilePath);
            }
            else if (options.CertContent != null)
            {
                certContent = options.CertContent;
            }
            else
            {
                throw new ArgumentException("Either certificate file path or certificate contents must be provided.", nameof(options));
            }

            certContent = certContent
                .Replace("\r", "")
                .Replace("\n", "")
                .Replace("-----BEGIN PRIVATE KEY-----", "")
                .Replace("-----END PRIVATE KEY-----", "");

            var key = CngKey.Import(Convert.FromBase64String(certContent), CngKeyBlobFormat.Pkcs8PrivateBlob);

            this.key = key ?? throw new ArgumentNullException(nameof(key));

            this.keyId = options.KeyId ?? throw new ArgumentNullException(nameof(options.KeyId),
                $"Make sure {nameof(ApnsJwtOptions)}.{nameof(options.KeyId)} is set to a non-null value.");

            this.teamId = options.TeamId ?? throw new ArgumentNullException(nameof(options.TeamId),
                $"Make sure {nameof(ApnsJwtOptions)}.{nameof(options.TeamId)} is set to a non-null value.");

            this.bundleId = options.BundleId ?? throw new ArgumentNullException(nameof(options.BundleId),
                $"Make sure {nameof(ApnsJwtOptions)}.{nameof(options.BundleId)} is set to a non-null value.");

            this.useSandbox = options.UseSandbox;
        }

        /// <summary>
        /// Constructs a client instance with given <paramref name="cert"/>
        /// for certificate-based authentication (using a .p12 certificate).
        /// </summary>
        public ApnsClient(HttpClient httpClient, X509Certificate cert) : this(httpClient)
        {
            if (cert == null)
            {
                throw new ArgumentNullException(nameof(cert));
            }

            var split = cert.Subject.Split(new[] { "0.9.2342.19200300.100.1.1=" }, StringSplitOptions.RemoveEmptyEntries);
            if (split.Length != 2)
            {
                // On Linux .NET Core cert.Subject prints `userId=xxx` instead of `0.9.2342.19200300.100.1.1=xxx`
                split = cert.Subject.Split(new[] { "userId=" }, StringSplitOptions.RemoveEmptyEntries);
            }

            if (split.Length != 2)
            {
                // if subject prints `uid=xxx` instead of `0.9.2342.19200300.100.1.1=xxx`
                split = cert.Subject.Split(new[] { "uid=" }, StringSplitOptions.RemoveEmptyEntries);
            }

            if (split.Length != 2)
            {
                throw new InvalidOperationException("Provided certificate does not appear to be a valid APNs certificate.");
            }

            var topic = split[1];
            this.isVoipCert = topic.EndsWith(bundleIdVoipSuffix);
            this.bundleId = split[1].Replace(bundleIdVoipSuffix, "");
            this.useCert = true;
        }

        public async Task<ApnsResponse> SendAsync(ApnsRequest push, CancellationToken ct = default)
        {
            Logger.Debug($"SendAsync to Token={push.Token} started...");

            if (this.useCert)
            {
                if (this.isVoipCert && push.Type != ApplePushType.Voip)
                {
                    throw new InvalidOperationException("Provided certificate can only be used to send 'voip' type pushes.");
                }
            }

            var payload = push.GeneratePayload();

            string url = (this.useSandbox ? DevelopmentEndpoint : ProductionEndpoint)
                + (this.useBackupPort ? ":2197" : ":443")
                + "/3/device/"
                + (push.Token ?? push.VoipToken);
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Version = new Version(2, 0);
            request.Headers.Add("apns-priority", push.Priority.ToString());
            request.Headers.Add("apns-push-type", push.Type.ToString().ToLowerInvariant());
            request.Headers.Add("apns-topic", this.GetTopic(push.Type));

            if (!this.useCert)
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("bearer", this.GetOrGenerateJwt());
            }

            if (push.Expiration.HasValue)
            {
                var exp = push.Expiration.Value;
                if (exp == DateTimeOffset.MinValue)
                    request.Headers.Add("apns-expiration", "0");
                else
                    request.Headers.Add("apns-expiration", exp.ToUnixTimeSeconds().ToString());
            }

            if (!string.IsNullOrEmpty(push.CollapseId))
            {
                request.Headers.Add("apns-collapse-id", push.CollapseId);
            }

            request.Content = new JsonContent(payload);

            HttpResponseMessage response;
            try
            {
                response = await this.httpClient.SendAsync(request, ct).ConfigureAwait(false);
            }
            catch (HttpRequestException ex) when (
                Environment.OSVersion.Platform is PlatformID.Win32NT &&
                ex.InnerException is AuthenticationException { InnerException: Win32Exception { NativeErrorCode: -2146893016 } } ||
                Environment.OSVersion.Platform is PlatformID.Unix &&
                ex.InnerException is IOException { InnerException: IOException { InnerException: IOException { InnerException: { InnerException: { HResult: 336151573 } } } } })
            {
                throw new ApnsCertificateExpiredException(ex);
            }

            var contentJson = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            // Process status codes specified by APNs documentation
            // https://developer.apple.com/documentation/usernotifications/setting_up_a_remote_notification_server/handling_notification_responses_from_apns
            var statusCode = response.StatusCode;
            if (statusCode == HttpStatusCode.OK)
            {
                Logger.Info($"SendAsync to Token={push.Token} successfully completed");
                return ApnsResponse.Successful;
            }

            // something went wrong
            // check for payload 
            // {"reason":"DeviceTokenNotForTopic"}
            // {"reason":"Unregistered","timestamp":1454948015990}

            ApnsErrorResponsePayload errorPayload;
            try
            {
                errorPayload = JsonConvert.DeserializeObject<ApnsErrorResponsePayload>(contentJson);
                Logger.Error($"SendAsync to Token={push.Token} failed with StatusCode={response.StatusCode}, Reason={errorPayload.Reason}");
            }
            catch (Exception ex)
            {
                Logger.Error($"SendAsync to Token={push.Token} failed with StatusCode={response.StatusCode}, Content={contentJson}", ex);
                return ApnsResponse.Error(ApnsResponseReason.Unknown, $"Status: {statusCode} ({(int)statusCode}), reason: {contentJson ?? "not specified"}.");
            }

            return ApnsResponse.Error(errorPayload.Reason, errorPayload.ReasonRaw);
        }

        public static ApnsClient CreateUsingCert(X509Certificate2 cert)
        {
#if NETSTANDARD2_0
            throw new NotSupportedException("Certificate-based connection is not supported on all .NET Framework versions and on .NET Core 2.x or lower.");
#elif NETSTANDARD2_1
            if (cert == null) throw new ArgumentNullException(nameof(cert));

            var handler = new HttpClientHandler();
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;

            handler.ClientCertificates.Add(cert);
            var httpClient = new HttpClient(handler);
            return new ApnsClient(httpClient, cert);
#endif
        }

        public static ApnsClient CreateUsingCert(string pathToCert, string certPassword = null)
        {
            if (string.IsNullOrWhiteSpace(pathToCert))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(pathToCert));
            }

            var cert = new X509Certificate2(pathToCert, certPassword);
            return CreateUsingCert(cert);
        }

        public ApnsClient UseSandbox()
        {
            this.useSandbox = true;
            return this;
        }

        /// <summary>
        /// Use port 2197 instead of 443 to connect to the APNs server.
        /// You might use this port to allow APNs traffic through your firewall but to block other HTTPS traffic.
        /// </summary>
        /// <returns></returns>
        public ApnsClient UseBackupPort()
        {
            this.useBackupPort = true;
            return this;
        }

        private string GetTopic(ApplePushType pushType)
        {
            switch (pushType)
            {
                case ApplePushType.Background:
                case ApplePushType.Alert:
                    return this.bundleId;
                case ApplePushType.Voip:
                    return this.bundleId + bundleIdVoipSuffix;
                case ApplePushType.Unknown:
                default:
                    throw new ArgumentOutOfRangeException(nameof(pushType), pushType, null);
            }
        }

        private string GetOrGenerateJwt()
        {
            lock (this.jwtRefreshLock)
            {
                if (this.lastJwtGenerationTime > DateTime.UtcNow - TimeSpan.FromMinutes(20)) // refresh no more than once every 20 minutes
                {
                    return this.jwt;
                }

                var now = DateTimeOffset.UtcNow;

                string header = JsonConvert.SerializeObject(new { alg = "ES256", kid = this.keyId });
                string payload = JsonConvert.SerializeObject(new { iss = this.teamId, iat = now.ToUnixTimeSeconds() });

                string headerBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(header));
                string payloadBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(payload));
                string unsignedJwtData = $"{headerBase64}.{payloadBase64}";

                byte[] signature;
                using (var dsa = new ECDsaCng(this.key))
                {
                    dsa.HashAlgorithm = CngAlgorithm.Sha256;
                    signature = dsa.SignData(Encoding.UTF8.GetBytes(unsignedJwtData));
                }

                this.jwt = $"{unsignedJwtData}.{Convert.ToBase64String(signature)}";
                this.lastJwtGenerationTime = now.UtcDateTime;
                return this.jwt;
            }
        }
    }
}