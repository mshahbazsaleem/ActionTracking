using Microsoft.AspNetCore.Identity;
using MAA.ActionTracking.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAA.ActionTracking.WebHost.Infrastructures
{
    public class UsernameAsPasswordValidator<TUser> : IPasswordValidator<TUser>
     where TUser : TenantUser
    {
        public Task<IdentityResult> ValidateAsync(UserManager<TUser> manager, TUser user, string password)
        {
            if (string.Equals(user.UserName, password, StringComparison.OrdinalIgnoreCase))
            {
                return Task.FromResult(IdentityResult.Failed(new IdentityError
                {
                    Code = "UsernameAsPassword",
                    Description = "You cannot use your username as your password"
                }));
            }
            return Task.FromResult(IdentityResult.Success);
        }
    }
}
