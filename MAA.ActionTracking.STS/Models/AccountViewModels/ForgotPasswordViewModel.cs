using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MAA.ActionTracking.WebHost.Models.AccountViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Prompt ="Enter your e-mail address", Name ="Email",Description ="You will be sent a new activation link")]
        public string Email { get; set; }
    }
}
