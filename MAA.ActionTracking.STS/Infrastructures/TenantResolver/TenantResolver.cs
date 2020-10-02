using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SaasKit.Multitenancy;
using MAA.ActionTracking.Data.Contexts;
using MAA.ActionTracking.Data.Entities;
using IdentityServer4.Services;
using MAA.ActionTracking.WebHost.Infrastructures.Extensions;

namespace MAA.ActionTracking.WebHost.Infrastructures.TenantResolver
{
    public class TenantResolver : ITenantResolver<Tenant>
    {

        private readonly ActionTrackingDbContext _context;
        // private readonly IIdentityServerInteractionService _interaction;

        public TenantResolver(ActionTrackingDbContext context)
        {
            _context = context;
            //_interaction = interaction;
        }


        public Task<TenantContext<Tenant>> ResolveAsync(HttpContext context)
        {
            //Identify tenant on the basis of AcrValues passed
            var returnUrl = context.Request.Query["returnUrl"];

            var tenantIdentifier = string.Empty;
            var database = string.Empty;

            tenantIdentifier = context.Request.Query.GetValue("acr_values", "tenant");// GetTenantFromAcrValues(context.Request.Query["acr_values"]);
            database = context.Request.Query.GetValue("acr_values", "idp");

            tenantIdentifier = "tenant2.localhost:6001";

            TenantContext<Tenant> tenantContext = null;
            Tenant tenant = null;

            if (context.Items["tenant"] != null)
            {
                tenant = context.Items["tenant"] as Tenant;
            }
            else
            {
                tenant = _context.Tenants.FirstOrDefault(t =>
                                t.TenantId.Equals(tenantIdentifier));
            }

            if (tenant != null)
            {
                tenantContext = new TenantContext<Tenant>(tenant);

                context.Items.Add("tenant", tenant);
            }

            return Task.FromResult(tenantContext);
        }
    }
}
