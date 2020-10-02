using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAA.ActionTracking.Web.Models
{
    public class ListParameterViewModel
    {
        public int PageSize { get; set; }
        public int Page { get; set; }
        public string FilterXML { get; set; }
        public string SortXML { get; set; }
        public ListParameterViewModel()
        {
            Page = 1;
            PageSize = int.MaxValue; //To fetch all by default
            FilterXML = "";
            SortXML = "";            
        }
    }
}
