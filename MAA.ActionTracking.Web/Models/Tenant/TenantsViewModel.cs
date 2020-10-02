using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAA.ActionTracking.Web.Models.Tenant
{
    public class TenantsViewModel:ParentListViewModel
    {
        public TenantsViewModel()
        {
            Data = new List<TenantViewModel>();
        }
        public List<TenantViewModel> Data {get;set;}
    }
}
