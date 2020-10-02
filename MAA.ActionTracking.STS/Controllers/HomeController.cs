using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAA.ActionTracking.WebHost.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            List<System.Security.Claims.Claim> claims = ((System.Security.Claims.ClaimsPrincipal)User).Claims.ToList();

            return View("/Index");
        }

    }
}
