using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MAA.ActionTracking.Data.Contexts;
using MAA.ActionTracking.Data.Entities;
using MAA.ActionTracking.WebHost.Models;
using MAA.ActionTracking.WebHost.Infrastructures.Configuration.Interfaces;

namespace MAA.ActionTracking.WebHost.Infrastructures.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddIdentityService(this IServiceCollection services, IConfiguration configuration, string migrationsAssembly)
        {
            services.AddIdentity<TenantUser, TenantRole>(options =>
            {
                // Password settings
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
            })
                .AddEntityFrameworkStores<ActionTrackingDbContext>()
                .AddUserManager<TenantUserManager>()
                .AddDefaultTokenProviders()
                .AddPasswordValidator<UsernameAsPasswordValidator<TenantUser>>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            var builder = services.AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;
            })
                .AddInMemoryIdentityResources(Config.GetIdentityResources())
                .AddInMemoryApiResources(Config.GetApiResources())
                .AddInMemoryApiScopes(Config.GetApiScopes())
                .AddInMemoryClients(Config.GetClients())
                .AddAspNetIdentity<TenantUser>();


            //builder.Services.AddScoped<IProfileService, ProfileService>();

            //TODO: Change it to actual certificate for production
            builder.AddDeveloperSigningCredential();

            return services;
        }


        public static IServiceCollection AddContexts(this IServiceCollection services, IConfiguration configuration, string migrationsAssembly)
        {
            services.AddSystemDataContext(configuration, migrationsAssembly)
               // .AddTenantDbContext(configuration, migrationsAssembly)
                .AddMultiTenantDbContext(configuration, migrationsAssembly);

            return services;
        }

        

    }
}
