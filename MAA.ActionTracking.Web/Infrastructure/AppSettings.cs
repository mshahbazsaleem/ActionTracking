using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAA.ActionTracking.Web.Infrastructure
{
    public class AppSettings
    {
        public string Authority { get; set; }
        public string AcrValues { get; set; }
        public string RedirectUri { get; set; }
        /// <summary>
        /// Maps to APIRootUri setting in appsettings.json
        /// </summary>
        public string APIRootUri { get; set; }
    
    }
}
