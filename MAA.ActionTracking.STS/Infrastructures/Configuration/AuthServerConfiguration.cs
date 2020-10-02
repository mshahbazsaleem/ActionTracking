using MAA.ActionTracking.WebHost.Infrastructures.Configuration.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAA.ActionTracking.WebHost.Infrastructures.Configuration
{
    public class AuthServerConfiguration : IAuthServerConfiguration
    {
        public string IdentityServerHost { get; set; } = "http://localhost:8000";

        public string RedirectUris { get; set; } = "http://localhost:5003";
        public string PostLogoutRedirectUris { get; set; } = "http://localhost:5003/signout";
        public string DataProtectionPath { get; set; } = string.Empty;
    }
}
