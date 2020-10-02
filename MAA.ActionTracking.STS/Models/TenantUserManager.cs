using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MAA.ActionTracking.WebHost.Infrastructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MAA.ActionTracking.Data.Entities;

namespace MAA.ActionTracking.WebHost.Models
{
    public class TenantUserManager : UserManager<TenantUser>
    {
        public string TenantId { get; set; }

        //private readonly Tenant _currentTenant;
        private readonly IEnumerable<IPasswordValidator<TenantUser>> _passwordValidators;
        public TenantUserManager(IUserStore<TenantUser> store, IOptions<IdentityOptions> optionsAccessor, IPasswordHasher<TenantUser> passwordHasher, IEnumerable<IUserValidator<TenantUser>> userValidators, IEnumerable<UsernameAsPasswordValidator<TenantUser>> passwordValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<TenantUser>> logger)
            : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
            //_currentTenant = tenant;
            _passwordValidators = passwordValidators;
        }

        //public virtual Task<TenantUser> FindByNameAndTenantAsync(string userName, string tenantId)
        //{
        //    return Task.FromResult(base.Users.Where(u => u.NormalizedUserName == userName.ToUpper().Trim() && u.TenantId == tenantId).SingleOrDefault());
        //}

        //public override Task<TenantUser> FindByNameAsync(string userName)
        //{
        //    return Task.FromResult(base.Users.Where(u => u.NormalizedUserName == userName.ToUpper().Trim() && u.TenantId == _currentTenant.TenantId).SingleOrDefault());
        //}

        //public override Task<TenantUser> FindByEmailAsync(string email)
        //{
        //    return Task.FromResult(base.Users.Where(u => u.Email == email.ToUpper().Trim() && u.TenantId == _currentTenant.TenantId).SingleOrDefault());
        //}

        //public override Task<TenantUser> FindByIdAsync(string userId)
        //{
        //    try
        //    {
        //        return base.FindByIdAsync(userId);
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //    //return Task.FromResult(base.Users.Where(u => u.Id == userId.Trim() && u.TenantId == this.TenantId).SingleOrDefault());
        //}
        //public override Task<IdentityResult> CreateAsync(TenantUser user, string password)
        //{
        //    if (user == null)
        //    {
        //        throw new ArgumentNullException("user");
        //    }

        //    user.TenantId = _currentTenant.TenantId;

        //    return base.CreateAsync(user,password);
        //}

        //public override Task<IdentityResult> ResetPasswordAsync(TenantUser user, string token, string newPassword)
        //{
        //    //TODO: Ist try to make it work from starup.cs otherwise implement custom validator

        //    //var result =  _passwordValidators.First().ValidateAsync(this, user, newPassword);
        //    //var result1 = base.ValidatePasswordAsync(user, newPassword).Result;
            
        //    return base.ResetPasswordAsync(user, token, newPassword);

        //}

    }
}
