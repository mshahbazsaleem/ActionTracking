using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAA.ActionTracking.WebHost.Infrastructures.Configuration.Interfaces
{
    public class IAuthServerConfiguration
    {
        string IdentityServerHost { get; }
        string RedirectUris { get; }
        string PostLogoutRedirectUris { get; }
        string DataProtectionPath { get; }
    }
}
