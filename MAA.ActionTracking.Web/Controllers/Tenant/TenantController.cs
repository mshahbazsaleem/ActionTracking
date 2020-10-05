using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MAA.ActionTracking.Web.Infrastructure;
using MAA.ActionTracking.Web.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MAA.ActionTracking.Web.Controllers
{
    //TODO: Uncomment once we are done with multi tenancy and 
    //role claims are properly returned by IDS
    //[Authorize(Policy = "AdminPolicy")]
    [Authorize]
    public class TenantController : BaseController
    {
        public TenantController(TenantService tenantService):base(tenantService)
        {

        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
