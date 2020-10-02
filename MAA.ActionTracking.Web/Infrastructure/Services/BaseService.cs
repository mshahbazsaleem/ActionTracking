using MAA.ActionTracking.Data.Contexts;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAA.ActionTracking.Web.Infrastructure.Services
{
    public class BaseService : UserProfileService
    {
        public BaseService(ActionTrackingDbContext dbContext, IOptionsSnapshot<AppSettings> appSettings, HttpContext httpContext) : base(dbContext, httpContext, appSettings)
        {
            AppSettings = appSettings.Value;
            HttpContext = httpContext;
            DbContext = dbContext;
        }

        public BaseService(ActionTrackingDbContext dbContext, IOptionsSnapshot<AppSettings> appSettings, HttpContext httpContext, ILogger logger) : base(dbContext, httpContext, appSettings)
        {
            AppSettings = appSettings.Value;
            HttpContext = httpContext;
            Logger = logger;
            DbContext = dbContext;
        }
        public AppSettings AppSettings { get; }
        public HttpContext HttpContext { get; }
        public ActionTrackingDbContext DbContext { get;  }
        public ILogger Logger { get; }
     
    }
}
