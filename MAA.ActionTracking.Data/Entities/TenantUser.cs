using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace MAA.ActionTracking.Data.Entities
{
    public class TenantUser : IdentityUser<int>
    {

        [Required]
        public string TenantId { get; set; }

        [Required]
        public int Status { get; set; } //1=> All Good, 2=> Blocked

        [Required]
        public DateTime CreatedDate { get; set; }

        [Required]
        public DateTime LastUpdated { get; set; }

        public bool HasPasswordSynced { get; set; }
    }
}
