using Alexinea.Autofac.Extensions.DependencyInjection;
using Autofac;
using AutoMapper;
using Finbuckle.MultiTenant;
using IdentityModel;
using IdentityModel.AspNetCore;
using MAA.ActionTracking.Web.Infrastructure.Middlewares;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace MAA.ActionTracking.Web.Infrastructure.Extensions
{
    public static class MiddelwareExtensions
    {
        public static void ConfigureCustomExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
        }
        public static void AddLogging(this IApplicationBuilder app, ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
        }
        public static void ConfigureCookiePolicy(this IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
        }

        public static void ConfigureDistributedCache(this IServiceCollection services, IWebHostEnvironment hostingEnvironment, IConfiguration configuration)
        {
            //var sqlConnectionString = configuration["ConnectionStrings:"];

            //if (hostingEnvironment.EnvironmentName.Equals("Design"))
            //{
                //TODO: Only use Memory Cache for development, configure Redis/SQL Server for production later
                services.AddDistributedMemoryCache();
            //}
            //else
            //{
            //    services.AddDistributedSqlServerCache(options =>
            //    {
            //        options.ConnectionString = sqlConnectionString;

            //        options.SchemaName = "dbo";
            //        options.TableName = "Cache";
            //    });
            //}
        }
        public static void ConfigureRazorViewEngine(this IServiceCollection services)
        {
            //services.Configure<RazorViewEngineOptions>(options =>
            //{
            //    options.AreaViewLocationFormats.Clear();
            //    options.AreaViewLocationFormats.Add("/Areas/{2}/Views/{1}/{0}.cshtml");
            //    options.AreaViewLocationFormats.Add("/Areas/{2}/Views/Shared/{0}.cshtml");
            //    options.AreaViewLocationFormats.Add("/Views/Shared/{0}.cshtml");
            //});
        }

        public static void ConfigureApiBehaviors(this IServiceCollection services)
        {
            /*
            Disable automatic 400
            To disable the automatic 400 behavior, 
            set the SuppressModelStateInvalidFilter property 
            to true. Add the following highlighted code in 
            Startup.ConfigureServices after 
            services.AddMvc().SetCompatibilityVersion:
           */
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
        }

        public static void ConfigureAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            var appSettings = configuration.GetSection("AppSettings");

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "Cookies";
                options.DefaultChallengeScheme = "oidc";
            })
            .AddCookie("Cookies")
            .AddAutomaticTokenManagement()
            .AddOpenIdConnect("oidc", options =>
            {
                options.SignInScheme = "Cookies";

                options.Authority = appSettings["Authority"]; 
                options.RequireHttpsMetadata = false;

                options.ClientId = appSettings["ClientId"];
                options.ClientSecret = "R#L0cked!!";
                options.ResponseType = "code id_token";//Get access code separately using back channel
                //options.ResponseType = "id_token";

                options.SaveTokens = true;
                options.GetClaimsFromUserInfoEndpoint = true;

                options.Scope.Clear();
                options.Scope.Add("openid");
                options.Scope.Add("profile");
                options.Scope.Add("email");
                options.Scope.Add("offline_access");
                options.Scope.Add("at_api");

                //options.ClaimActions.Add(new MapAllClaimsAction());
                //options.ClaimActions.DeleteClaim(JwtClaimTypes.Role);
                //options.ClaimActions.MapJsonKey(JwtClaimTypes.Role, JwtClaimTypes.Role, JwtClaimTypes.Role);

                //TODO: remove after claims verification
                options.ClaimActions.Remove("amr");
                options.ClaimActions.MapUniqueJsonKey("sub", "sub");
                options.ClaimActions.MapUniqueJsonKey("name", "name");
                options.ClaimActions.MapUniqueJsonKey("given_name", "given_name");
                options.ClaimActions.MapUniqueJsonKey("family_name", "family_name");
                options.ClaimActions.MapUniqueJsonKey("profile", "profile");
                options.ClaimActions.MapUniqueJsonKey("email", "email");
                options.ClaimActions.MapUniqueJsonKey("website", "website");
                //

                options.Events = new OpenIdConnectEvents
                {
                    OnMessageReceived = OnMessageReceived,
                    OnRedirectToIdentityProvider = n => OnRedirectToIdentityProvider(n, appSettings)
                    // OnRedirectToIdentityProvider = n => OnRedirectToIdentityProvider(n, tenntConfiguration),
                };

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = JwtClaimTypes.Name,
                    RoleClaimType = JwtClaimTypes.Role,
                };
            });
            //services.AddTransient<IClaimsTransformation, ClaimsTransformer>();
            #region From IDS4 Docs
            /*JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "Cookies";
                options.DefaultChallengeScheme = "oidc";
            })
                .AddCookie("Cookies")
                .AddOpenIdConnect("oidc", options =>
                {
                    options.SignInScheme = "Cookies";

                    options.Authority = "http://localhost:5000";
                    options.RequireHttpsMetadata = false;

                    options.ClientId = "mvc";
                    options.ClientSecret = "secret";
                    options.ResponseType = "code id_token";

                    options.SaveTokens = true;
                    options.GetClaimsFromUserInfoEndpoint = true;

                    options.Scope.Add("api1");
                    options.Scope.Add("offline_access");

                    options.ClaimActions.MapJsonKey("website", "website");
                });*/
            #endregion From IDS4 Docs
        }
        private static Task OnMessageReceived(MessageReceivedContext context)
        {
            context.Properties.IsPersistent = true;
            context.Properties.ExpiresUtc = new DateTimeOffset(DateTime.Now.AddHours(12));

            return Task.FromResult(0);
        }

        private static Task OnRedirectToIdentityProvider(RedirectContext n, IConfigurationSection appSettings)
        {
            var ti = n.HttpContext.GetMultiTenantContext<TenantInfo>()?.TenantInfo;
            //TODO: Inject tenant configuration here for acr values 
            n.ProtocolMessage.AcrValues = appSettings["AcrValues"]; 
            n.ProtocolMessage.RedirectUri = appSettings["RedirectUri"]; 

            return Task.FromResult(0);
        }
        public static void ConfigureSession(this IServiceCollection services)
        {
            services.AddSession(options =>
            {
                options.Cookie.Name = ".ac.Session";
                // Set a short timeout for easy testing.
                options.IdleTimeout = TimeSpan.FromMinutes(10);
                options.Cookie.HttpOnly = true;
                // Make the session cookie essential
                options.Cookie.IsEssential = true;
            });
        }

        public static void ConfigureAppSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AppSettings>(configuration.GetSection("AppSettings"));
        }
        public static IServiceProvider RegisterDependencies(this IServiceCollection services)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            services.AddAutoMapper(typeof(Startup));

            //// Application services
            //services.AddScoped<FilesHelper, FilesHelper>();         


            //Autofac for DI container
            var builder = new ContainerBuilder();

            builder.RegisterType<HttpContextAccessor>().As<IHttpContextAccessor>().SingleInstance();

            builder.Register(ctx =>
            {
                return ctx.Resolve<IHttpContextAccessor>().HttpContext;

            }).As<HttpContext>();

            //Register services
            builder
                .RegisterAssemblyTypes(typeof(Startup).Assembly)
                .Where(t => t.Namespace.StartsWith("MAA.ActionTracking.Web.Infrastructure.Services"));

            //builder
            //    .RegisterGeneric(typeof(BaseService<>))
            //    .InstancePerDependency();

            builder.Populate(services);

            var container = builder.Build();

            return container.Resolve<IServiceProvider>();
        }
    }
}
