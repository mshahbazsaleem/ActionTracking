using Microsoft.AspNetCore.Mvc.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAA.ActionTracking.Web.Models
{
    public class GridActionModel
    {
        public string Name { get; set; }
        public LocalizedHtmlString Title { get; set; }
        
    }
}
