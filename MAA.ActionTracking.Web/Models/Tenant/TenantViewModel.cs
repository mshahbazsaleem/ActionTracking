﻿using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAA.ActionTracking.Web.Models.Tenant
{
    public class TenantViewModel: Data.Entities.Tenant
    {
        public IEnumerable<SelectListItem> SystemVariables { get; set; }
    }
}
