using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Services;
using MAA.ActionTracking.Data.Entities;
using MAA.ActionTracking.Data.Entities;
using MAA.ActionTracking.WebHost.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MAA.ActionTracking.WebHost.Infrastructures
{
    /// <summary>
    /// This class is not required in normal circumestances
    /// and just added to explore the customization options
    /// for IdentityServer4. This can be customized to add
    /// additional claims required by the application
    /// </summary>
    public class ProfileService : IProfileService
    {
        private readonly TenantUserManager _tenantUserManager;
        private readonly Tenant _currentTenant;
        public ProfileService(TenantUserManager tenantUserManager, Tenant tenant)
        {
            _tenantUserManager = tenantUserManager;
            _currentTenant = tenant;
        }
        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            await Task.Run(() =>
            {
                if (context.Subject.Identities.First().Claims.Any())
                {

                    var claims = context.Subject.Identities.First().Claims;
                    //if (context.RequestedClaimTypes != null && context.RequestedClaimTypes.Any())
                    //{
                    //    claims = claims.Where(x => context.RequestedClaimTypes.Contains(x.Type)).ToArray().AsEnumerable();
                    //}

                    var finalClaims = claims.ToList();

                    //Check why tenant is null
                    if (_currentTenant != null)
                    {
                        finalClaims.Add(new Claim("tenant_id", _currentTenant.TenantId));
                    }

                    context.IssuedClaims = finalClaims;
                }
            });

        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            return Task.FromResult(0);
        }

        private async Task<ClaimsPrincipal> GetClaims(TenantUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var id = new ClaimsIdentity();
            id.AddClaim(new Claim(JwtClaimTypes.PreferredUserName, user.UserName));

            id.AddClaims(await _tenantUserManager.GetClaimsAsync(user));

            return new ClaimsPrincipal(id);
        }

    }
}
