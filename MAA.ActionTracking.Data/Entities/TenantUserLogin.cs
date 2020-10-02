using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace MAA.ActionTracking.Data.Entities
{
    public class TenantUserLogin : IdentityUserLogin<int>
    {
        [Key]
        public new int UserId { get; set; }
    }
}
