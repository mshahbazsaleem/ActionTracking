using MAA.ActionTracking.Data.Contexts;
using MAA.ActionTracking.Data.Entities;
using MAA.ActionTracking.Infrastructure;
using MAA.ActionTracking.Infrastructure.Extensions;
using MAA.ActionTracking.Web.Infrastructure.Helpers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Razor.Language.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MAA.ActionTracking.Web.Infrastructure.Services
{
    public class UserProfileService
    {
        private readonly HttpContext _httpContext;
        private readonly AppSettings _appSettings;
        private readonly ActionTrackingDbContext _dbContext;
        public UserProfileService(ActionTrackingDbContext dbContext, HttpContext httpContext,  IOptionsSnapshot<AppSettings> options)
        {
            _httpContext = httpContext;
            _appSettings = options.Value;
            _dbContext = dbContext;
        }

        public HttpClient HttpClient
        {
            get
            {
                var logger = _httpContext.RequestServices.GetService(typeof(ILogger<HttpLoggingHandler>)) as ILogger<HttpLoggingHandler>;

                //Set bearer token for api access
                var accessToken = _httpContext.GetTokenAsync("access_token").Result;

                var client = new HttpClient(new HttpLoggingHandler(logger)) { BaseAddress = new Uri(_appSettings.APIRootUri) };

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                return client;
            }
        }

        public ClaimsPrincipal User
        {
            get
            {
                return _httpContext.User;
            }
        }
        public string LoggedInUsername
        {
            get
            {
                return User.Identity.IsAuthenticated
                        ? (User
                            .Claims
                            .FirstOrDefault(c => c.Type.Equals("name"))
                            .Value)
                        : string.Empty;
            }
        }
        public string UserFirstName
        {
            get
            {
                return User.Identity.IsAuthenticated
                        ? (User
                            .Claims
                            .FirstOrDefault(c => c.Type.Equals("first_name"))
                            .Value)
                        : string.Empty;
            }
        }
        public string UserLastName
        {
            get
            {
                return User.Identity.IsAuthenticated
                        ? (User
                            .Claims
                            .FirstOrDefault(c => c.Type.Equals("last_name"))
                            .Value)
                        : string.Empty;
            }
        }
        public int TenantId
        {
            get
            {
                return Convert.ToInt32(
                            User.Identity.IsAuthenticated
                            ? (User
                                .Claims
                                .FirstOrDefault(c => c.Type.Equals("tenant_id"))
                                .Value)
                            : "-1");
            }
        }

      public TenantUser UserProfile {
            get {
                return GetUserProfile().Result;
            }

        }
        public async Task<TenantUser> GetUserProfile()
        {
            TenantUser model = await _httpContext.Session.GetAsync<TenantUser>(SessionKeys.UserProfile);

            if (model != null) return model;

            model = _dbContext.Users.FirstOrDefault(u => u.UserName.Equals(LoggedInUsername));

            if(model!=null)
                await _httpContext.Session.SetAsync<TenantUser>(SessionKeys.UserProfile, model);

            return model;
        }

    }
}
