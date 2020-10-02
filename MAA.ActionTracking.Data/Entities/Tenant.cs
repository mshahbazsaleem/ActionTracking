using Finbuckle.MultiTenant;
using MAA.ActionTracking.Data.Abstracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MAA.ActionTracking.Data.Entities
{
    public class TenantInfo
    {
        public string Id { get; set; }

       public string Name { get; set; }

        public string Identifier { get; set; }

        public string ConnectionString { get; set; }
    }

    public class Tenant : EntityBase
    {
        public Tenant()
        {
            CreatedDate = DateTime.Now;
            LastUpdated = DateTime.Now;
        }

        [Required]
        public string TenantId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string HostName { get; set; }
     
        public string ApplicationTitle { get; set; }

        public string FooterEmail { get; set; }

        public string FooterHelpText { get; set; }
  
        public string TenantLogo { get; set; }
    }
}
