using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MAA.ActionTracking.Data.Contexts;

namespace MAA.ActionTracking.WebHost.Infrastructures.Extensions
{
    public static class DbContextExtension
    {

        public static IServiceCollection AddSystemDataContext(this IServiceCollection services, IConfiguration configuration, string migrationAssembly)
        {
            services.AddDbContext<ActionTrackingDbContext>(builder =>
                builder.UseSqlServer(configuration.GetConnectionString("SystemConnection"),
                    options => options.MigrationsAssembly(migrationAssembly)));
            return services;
        }


        //public static IServiceCollection AddTenantDbContext(this IServiceCollection services, IConfiguration configuration, string migrationAssembly)
        //{

        //    services.AddDbContext<TenantDbContext>();

        //    services.AddDbContext<TenantDbContext>(builder =>
        //       builder.UseSqlServer(configuration.GetConnectionString("SystemConnection"),
        //           options => options.MigrationsAssembly(migrationAssembly)));
        //    return services;
        //}

        public static IServiceCollection AddMultiTenantDbContext(this IServiceCollection services, IConfiguration configuration, string migrationAssembly)
        {

            services.AddDbContext<MultiTenantStoreDbContext>();

            services.AddDbContext<MultiTenantStoreDbContext>(builder =>
               builder.UseSqlServer(configuration.GetConnectionString("SystemConnection"),
                   options => options.MigrationsAssembly(migrationAssembly)));
            return services;
        }


    }
}
