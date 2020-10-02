using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Finbuckle.MultiTenant;
using MAA.ActionTracking.Data.Contexts;
using MAA.ActionTracking.Web.Infrastructure.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

namespace MAA.ActionTracking.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment hostingEnvironment)
        {
            var builder = new ConfigurationBuilder()
             .SetBasePath(hostingEnvironment.ContentRootPath)
             .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
             .AddJsonFile($"appsettings.{hostingEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: true)
             .AddEnvironmentVariables();

            Configuration = builder.Build();

            HostingEnvironment = hostingEnvironment;
        }

        public IConfiguration Configuration { get; }
        private IWebHostEnvironment HostingEnvironment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {

            services.AddControllersWithViews().AddNewtonsoftJson(options =>
            {
                options.UseMemberCasing();
            });

            services.AddDbContext<ActionTrackingDbContext>(builder =>
              builder.UseSqlServer(Configuration.GetConnectionString("SystemConnection")));

            services.AddMultiTenant<TenantInfo>()
                  .WithEFCoreStore<MultiTenantStoreDbContext, TenantInfo>()
                  .WithRouteStrategy();

            //Runtime compilation
#if DEBUG
            services.AddRazorPages()
                    .AddRazorRuntimeCompilation();
#endif

            //Cookie policies
            services.ConfigureCookiePolicy();

            //Distributed cache
            services.ConfigureDistributedCache(HostingEnvironment, Configuration);

            services
                .AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                .AddNewtonsoftJson(options =>
                {
                    options.UseMemberCasing();
                });


            //Binder customization for grid view model
            //services.AddControllers(options =>
            //{
            //    options.ModelBinderProviders.Insert(0, new GridViewModelBinderProvider());

            //}).AddNewtonsoftJson();


            //Configure Api behavior
            services.ConfigureApiBehaviors();

            //Configure application settings
            services.ConfigureAppSettings(Configuration);

            //Configure Session
            services.ConfigureSession();

            //Configure Authentication
            services.ConfigureAuthentication(Configuration);

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MAA API", Version = "v1" });
            });

            // Add authorization policies for MVC
            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminPolicy",
                    policy => policy.RequireRole("CentralAdmin"));
            });
            //Dependency injection
            return services.RegisterDependencies();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            app.AddLogging(loggerFactory, Configuration);         

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                // Enable middleware to serve generated Swagger as a JSON endpoint.
                app.UseSwagger();

                // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
                // specifying the Swagger JSON endpoint.
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "MAA API V1");
                    c.RoutePrefix = string.Empty;
                });
            }
            else
            {
                app.UseDeveloperExceptionPage();
                app.UseHsts();
            }

            //Plug-in tenant identification into the pipeline
            //app.UseTenantVerificationMiddleware(appSettings);

            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseSession();
            app.ConfigureCustomExceptionMiddleware();

            app.UseRouting();
            app.UseAuthorization();
            app.UseMultiTenant();

            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapControllerRoute("defaut", "{__tenant__=}/{controller=Home}/{action=Index}");
                endpoints.MapControllerRoute("defaut", "{controller=Home}/{action=Index}");
            });
            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllerRoute(
            //        name: "default",
            //        pattern: "{controller=Home}/{action=Index}/{id?}");
            //});

        }
    }
}