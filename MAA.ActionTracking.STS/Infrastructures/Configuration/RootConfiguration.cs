using MAA.ActionTracking.WebHost.Infrastructures.Configuration.Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAA.ActionTracking.WebHost.Infrastructures.Configuration
{
    public class RootConfiguration : IRootConfiguration
    {
        public IAuthServerConfiguration AuthServerConfiguration { get; set; }

        public RootConfiguration(IOptions<AuthServerConfiguration> authServerConfiguration)
        {
            AuthServerConfiguration = authServerConfiguration.Value;
        }
    }
}
