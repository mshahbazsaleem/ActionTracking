using MAA.ActionTracking.Constants;
using MAA.ActionTracking.Data.Entities;
using MAA.ActionTracking.Data.Infrastrutures.Extensions;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MAA.ActionTracking.Data.Contexts
{
    public class ActionTrackingDbContext : IdentityDbContext<TenantUser, TenantRole, int, TenantUserClaim, TenantUserRole, TenantUserLogin, TenantUserRoleClaim, TenantUserToken>
    {

        public ActionTrackingDbContext(DbContextOptions<ActionTrackingDbContext> options)
            :base(options)
        {
            
        }

        public DbSet<TenantInfo> TenantInfos { get; set; }
        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<TestVariable> TestVariables { get; set; }
        public DbSet<SystemVariable> SystemVariables { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            ConfigureIdentityContext(modelBuilder);
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
