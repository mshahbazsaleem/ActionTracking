using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Finbuckle.MultiTenant;
using MAA.ActionTracking.Data.Contexts;
using MAA.ActionTracking.Data.Entities;
using MAA.ActionTracking.STS.Infrastructures;
using MAA.ActionTracking.WebHost.Infrastructures;
using MAA.ActionTracking.WebHost.Infrastructures.Extensions;
using MAA.ActionTracking.WebHost.Infrastructures.TenantResolver;
using MAA.ActionTracking.WebHost.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing.Patterns;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using TenantInfo = Finbuckle.MultiTenant.TenantInfo;

namespace MAA.ActionTracking.STS
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {            
            IdentityModelEventSource.ShowPII = true;

            var migrationAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
            services.AddContexts(Configuration, migrationAssembly);

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.Lax;
            });

            //Two options available, resolved tenant from db in each request or from cache
            services.AddMultitenancy<Tenant, MemoryCacheTenantResolver>();
            //services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient(typeof(Tenant),typeof (Tenant));

            //services.AddMultiTenant<TenantInfo>()
            //       .WithEFCoreStore<MultiTenantStoreDbContext, TenantInfo>()
            //       .WithStrategy<QueryStringMultiTenantStrategy>(ServiceLifetime.Transient);

            services.AddTransient<IPasswordValidator<TenantUser>, UsernameAsPasswordValidator<TenantUser>>();

            services.AddIdentityService(Configuration, migrationAssembly);

            services.AddAuthentication();

            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();
            services.AddDataProtection()
                .SetApplicationName("ActionTracking")
                .PersistKeysToFileSystem(new System.IO.DirectoryInfo(Configuration["DataProtectionPath"]));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseMultitenancy<Tenant>();
            //app.UseMultiTenant();
            app.UseIdentityServer();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                   name: "default",
                   pattern: "{controller=Home}/{action=Index}/{id?}");
                //endpoints.MapControllerRoute("default", "{first_segment=}/{controller=Home}/{action=Index}");
            });
        }
    }
}
