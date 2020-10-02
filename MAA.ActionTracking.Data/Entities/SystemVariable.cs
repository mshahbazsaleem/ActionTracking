using MAA.ActionTracking.Data.Abstracts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace MAA.ActionTracking.Data.Entities
{
    public class SystemVariable:EntityBase
    {
        public SystemVariable()
        {
            CreatedDate = DateTime.Now;
            LastUpdated = DateTime.Now;
        }
        public string Name { get; set; }
        public string Value { get; set; }
        public string TenantId { get; set; }
    }
}
