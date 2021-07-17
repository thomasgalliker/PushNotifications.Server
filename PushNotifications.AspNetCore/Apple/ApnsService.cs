using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using PushNotifications.Apple;

namespace PushNotifications.AspNetCore
{
    public class ApnsService : IApnsService
    {
        private readonly IApnsClientFactory apnsClientFactory;
        private readonly ApnsJwtOptions apnsJwtOptions;
        private readonly ApnsServiceOptions options;
        private readonly IApnsClient client;

        // TODO implement expiration policy
        private readonly ConcurrentDictionary<string, IApnsClient> cachedCertClients = new ConcurrentDictionary<string, IApnsClient>(); // key is cert thumbprint and sandbox prefix
        private readonly ConcurrentDictionary<string, IApnsClient> cachedJwtClients = new ConcurrentDictionary<string, IApnsClient>(); // key is bundle id and sandbox prefix

        public ApnsService(IApnsClientFactory apnsClientFactory, IOptions<ApnsServiceOptions> options, IOptions<ApnsJwtOptions> apnsJwtOptions)
        {
            this.apnsClientFactory = apnsClientFactory;
            this.apnsJwtOptions = apnsJwtOptions.Value;
            this.options = options.Value;

            this.client = this.apnsClientFactory.CreateUsingJwt(this.apnsJwtOptions, this.apnsJwtOptions.UseSandbox, this.options.DisableServerCertificateValidation);
        }

        public Task<ApnsResponse> SendAsync(ApplePush push)
        {
            return this.client.SendAsync(push);
        }

        public Task<ApnsResponse> SendPush(ApplePush push, X509Certificate2 cert, bool useSandbox = false)
        {
            string clientCacheId = (useSandbox ? "s_" : "") + cert.Thumbprint;
            var client = this.cachedCertClients.GetOrAdd(clientCacheId, _ => this.apnsClientFactory.CreateUsingCert(cert, useSandbox, this.options.DisableServerCertificateValidation));

            try
            {
                return client.SendAsync(push);
            }
            catch
            {
                this.cachedCertClients.TryRemove(clientCacheId, out _);
                throw;
            }
        }

        public Task<ApnsResponse> SendPush(ApplePush push, ApnsJwtOptions jwtOptions, bool useSandbox = false)
        {
            string clientCacheId = (useSandbox ? "s_" : "") + jwtOptions.BundleId;
            var client = this.cachedJwtClients.GetOrAdd(clientCacheId, _ => this.apnsClientFactory.CreateUsingJwt(jwtOptions, useSandbox, this.options.DisableServerCertificateValidation));
            try
            {
                return client.SendAsync(push);
            }
            catch
            {
                this.cachedJwtClients.TryRemove(clientCacheId, out _);
                throw;
            }
        }

        public async Task<List<ApnsResponse>> SendPushes(IReadOnlyCollection<ApplePush> pushes, X509Certificate2 cert, bool useSandbox = false) //TODO implement concurrent sendings
        {
            if (string.IsNullOrWhiteSpace(cert.Thumbprint))
            {
                throw new InvalidOperationException("Certificate does not have a thumbprint.");
            }

            string clientCacheId = (useSandbox ? "s_" : "") + cert.Thumbprint;
            var client = this.cachedCertClients.GetOrAdd(clientCacheId, _ => this.apnsClientFactory.CreateUsingCert(cert, useSandbox, this.options.DisableServerCertificateValidation));

            var result = new List<ApnsResponse>(pushes.Count);
            try
            {
                foreach (var push in pushes)
                {
                    result.Add(await client.SendAsync(push));
                }

                return result;
            }
            catch
            {
                this.cachedCertClients.TryRemove(cert.Thumbprint, out _);
                throw;
            }
        }

        public async Task<List<ApnsResponse>> SendPushes(IReadOnlyCollection<ApplePush> pushes, ApnsJwtOptions jwtOptions, bool useSandbox = false)
        {
            string clientCacheId = (useSandbox ? "s_" : "") + jwtOptions.BundleId;
            var client = this.cachedJwtClients.GetOrAdd(clientCacheId, _ => this.apnsClientFactory.CreateUsingJwt(jwtOptions, useSandbox, this.options.DisableServerCertificateValidation));
           
            var result = new List<ApnsResponse>(pushes.Count);

            try
            {
                foreach (var push in pushes)
                {
                    result.Add(await client.SendAsync(push));
                }

                return result;
            }
            catch
            {
                this.cachedJwtClients.TryRemove(clientCacheId, out _);
                throw;
            }
        }
    }
}
