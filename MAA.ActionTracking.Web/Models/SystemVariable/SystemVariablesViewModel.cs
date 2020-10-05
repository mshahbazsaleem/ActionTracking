using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAA.ActionTracking.Web.Models.SystemVariable
{
    public class SystemVariablesViewModel:ParentListViewModel
    {
        public SystemVariablesViewModel()
        {
            Data = new List<SystemVariableViewModel>();
        }
        public List<SystemVariableViewModel> Data {get;set;}
    }
}
