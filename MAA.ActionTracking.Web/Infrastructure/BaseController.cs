using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MAA.ActionTracking.Web.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace MAA.ActionTracking.Web.Infrastructure
{
    public class BaseController : Controller
    {
        UserProfileService _userProfileService;
        public BaseController(UserProfileService userProfileService)
        {
            _userProfileService = userProfileService;
        }
    }
}
