using Microsoft.AspNetCore.Mvc.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAA.ActionTracking.Web.Models
{
    public class ModalViewModel
    {
        public string Name { get; set; }
        public string Size { get; set; }//sm, md, xl
        /// <summary>
        /// Css class names separated by space.
        /// </summary>
        public string AdditionalClasses { get; set; }
        /// <summary>
        /// Knockout binding e.g.
        /// data-bind=css:{condition:'class'}
        /// </summary>
        public string Binding { get; set; }
        public  string Title { get; set; }
    }
}
