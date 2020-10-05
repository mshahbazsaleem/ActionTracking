using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAA.ActionTracking.Web.Models.TestVariable
{
    public class TestVariablesViewModel:ParentListViewModel
    {
        public TestVariablesViewModel()
        {
            Data = new List<TestVariableViewModel>();
        }
        public List<TestVariableViewModel> Data {get;set;}
    }
}
