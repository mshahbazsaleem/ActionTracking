using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using MAA.ActionTracking.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using MAA.ActionTracking.Constants;
using MAA.ActionTracking.Data.Infrastrutures.Extensions;

namespace MAA.ActionTracking.Data.Contexts
{
    /// <summary>
    /// ActionTrackingDbContext is fully customized
    /// table names will not have the AspNet prefix
    /// </summary>
    public sealed class ActionTrackingDbContexta : IdentityDbContext<TenantUser, TenantRole, int, TenantUserClaim, TenantUserRole, TenantUserLogin, TenantUserRoleClaim, TenantUserToken>
    {
        //private readonly Tenant _currentTenant;
        //public ActionTrackingDbContext(Tenant currentTenant)
        //{
        //    _currentTenant = currentTenant;

        //    if (_currentTenant != null)
        //    {
        //        //Database.EnsureCreated();
        //    }
        //}

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (_currentTenant != null)
        //    {
        //        optionsBuilder.UseSqlServer(_currentTenant.GetConnectionString(), options => options.MigrationsAssembly("MAA.ActionTracking.STS"));
        //    }

        //    base.OnConfiguring(optionsBuilder);
        //}

        ////Uncomment the following constructor to enable migrations
       // public ActionTrackingDbContext(DbContextOptions<ActionTrackingDbContext> options)
       //: base(options)
       // {

       // }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            ConfigureIdentityContext(builder);
        }

        private void ConfigureIdentityContext(ModelBuilder builder)
        {
            builder.Entity<TenantRole>().ToTable(TableConstants.IdentityRoles);
            builder.Entity<TenantUserRoleClaim>().ToTable(TableConstants.IdentityRoleClaims);
            builder.Entity<TenantUserRole>().ToTable(TableConstants.IdentityUserRoles);

            builder.Entity<TenantUser>().ToTable(TableConstants.IdentityUsers);
            builder.Entity<TenantUserLogin>().ToTable(TableConstants.IdentityUserLogins);
            builder.Entity<TenantUserClaim>().ToTable(TableConstants.IdentityUserClaims);
            builder.Entity<TenantUserToken>().ToTable(TableConstants.IdentityUserTokens);
        }

    }
}
