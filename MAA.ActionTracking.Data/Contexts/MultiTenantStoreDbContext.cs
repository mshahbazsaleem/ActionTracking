using Finbuckle.MultiTenant;
using Finbuckle.MultiTenant.Stores;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MAA.ActionTracking.Data.Contexts
{
    public class MultiTenantStoreDbContext : EFCoreStoreDbContext<TenantInfo>
    {
        public MultiTenantStoreDbContext(DbContextOptions options) : base(options)
        {
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer()
        //    base.OnConfiguring(optionsBuilder);
        //}
    }
}
