using Microsoft.AspNetCore.Mvc.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAA.ActionTracking.Web.Models
{
    public class ConfirmationViewModel
    {
        public string Id { get; set; } = "confirm-action";
        public string ActionButtonClass = "btn-success";
        public string Title { get; set; }
        public string Text { get; set; }
        public string ConfirmButtonText { get; set; }
        public string CancelButtonText { get; set; }

        public string ConfirmIcon { get; set; }
        public string CancelIcon{get;set;}
    }
}
