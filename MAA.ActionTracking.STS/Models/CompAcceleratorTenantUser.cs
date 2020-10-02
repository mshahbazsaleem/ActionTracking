using MAA.ActionTracking.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAA.ActionTracking.WebHost.Models
{
    public class CompAcceleratorTenantUser : TenantUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
