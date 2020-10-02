using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAA.ActionTracking.WebHost.Infrastructures.Configuration.Interfaces
{
    public interface IRootConfiguration
    {
        IAuthServerConfiguration AuthServerConfiguration { get; }
    }
}
