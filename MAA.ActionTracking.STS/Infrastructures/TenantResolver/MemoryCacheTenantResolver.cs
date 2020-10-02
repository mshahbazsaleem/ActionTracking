using IdentityServer4.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using MAA.ActionTracking.Data.Contexts;
using MAA.ActionTracking.Data.Entities;
using MAA.ActionTracking.WebHost.Infrastructures.Extensions;
using SaasKit.Multitenancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAA.ActionTracking.WebHost.Infrastructures.TenantResolver
{
    public class MemoryCacheTenantResolver : MemoryCacheTenantResolver<Tenant>
    {
        private readonly ActionTrackingDbContext _context;
        private readonly IIdentityServerInteractionService _interaction;
        public MemoryCacheTenantResolver(IMemoryCache cache, ILoggerFactory loggerFactory, IIdentityServerInteractionService interaction, ActionTrackingDbContext context)
            : base(cache, loggerFactory)
        {
            _context = context;
            _interaction = interaction;
        }

        protected override string GetContextIdentifier(HttpContext context)
        {
            return context.Request.Host.Value.ToLower();
        }

        protected override IEnumerable<string> GetTenantIdentifiers(TenantContext<Tenant> context)
        {
            return new string[] { context.Tenant.TenantId };
        }

        protected override MemoryCacheEntryOptions CreateCacheEntryOptions()
        {
            return new MemoryCacheEntryOptions()
                        .SetAbsoluteExpiration(new TimeSpan(1, 0, 0, 0)); // Cache for a day
        }

        protected override Task<TenantContext<Tenant>> ResolveAsync(HttpContext context)
        {
            //Identify tenant on the basis of AcrValues passed
            var returnUrl = context.Request.Query["returnUrl"];
            var logoutId = context.Request.Query["logoutId"];

            CustomAuthorizationContext authContext = null;


            if (string.IsNullOrEmpty(returnUrl))
            {
                authContext = context.Request.Query.GetAuthorizationContextFromAcrValues();
            }
            else
            {
                authContext = context.Request.Query.GetAuthorizationContextFromReturnUrl();
            }

            if (authContext == null && !string.IsNullOrEmpty(logoutId))//If its still null try to get from interaction service logoutid
            {
                var logout = _interaction.GetLogoutContextAsync(logoutId).Result;

                if (logout != null)
                {
                    var acrValues = logout.Parameters["acr_values"];

                    authContext = new CustomAuthorizationContext
                    {
                        Tenant = acrValues.Split(' ').First().Split(':').Last()
                    };
                }

            }

            TenantContext<Tenant> tenantContext = null;

            if (authContext != null)
            {
                var tenantIdentifier = $"{authContext?.Tenant}";
                var tenant = _context.Tenants.FirstOrDefault(t =>
                                t.TenantId.Equals(tenantIdentifier));

                if (tenant != null)
                {
                    tenantContext = new TenantContext<Tenant>(tenant);
                }
                else
                {
                    tenant = _context.Tenants.FirstOrDefault(t =>
                                t.TenantId.Equals(tenantIdentifier));

                    tenantContext = new TenantContext<Tenant>(tenant);
                }
            }

            return Task.FromResult(tenantContext);
        }

        private string GetTenantFromAcrValues(string acrValues)
        {
            if (!string.IsNullOrEmpty(acrValues))
                return acrValues.Replace("tenant:", string.Empty);

            return string.Empty;
        }
    }
}
