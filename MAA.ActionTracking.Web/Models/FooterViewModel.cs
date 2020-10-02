using Microsoft.AspNetCore.Mvc.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAA.ActionTracking.Models
{
    public class FooterViewModel
    {
        public bool ShowSaveAndNew { get; set; }
        public string PagerKey { get; set; }
        public string PagerValue { get; set; }

        public string ActionButtonText { get; set; }
        public string ActionButtonProcessingText { get; set; }
        public string ActionButtonIcon { get; set; }

    }
}
