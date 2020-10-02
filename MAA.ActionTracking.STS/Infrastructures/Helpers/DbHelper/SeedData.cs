using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MAA.ActionTracking.Data.Contexts;
using MAA.ActionTracking.Data.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using MAA.ActionTracking.Common.Enums;

namespace MAA.ActionTracking.WebHost.Infrastructures.Helpers.DbHelper
{
    public class SeedData
    {
        private static IConfiguration _configuration;
        public SeedData(IConfiguration configuration)
        {
            _configuration = configuration;
        }       

        public static void EnsureSeedData(IServiceProvider serviceProvider)
        {
            Console.WriteLine("Seeding database...");

            using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {

                var context = scope.ServiceProvider.GetRequiredService<ActionTrackingDbContext>();
                //context.Database.Migrate();
                EnsureSeedData(context);
            }

            Console.WriteLine("Done seeding database.");
            Console.WriteLine();
        }

        private static void EnsureSeedData(ActionTrackingDbContext context)
        {
            if (context.Tenants.Count() == 0)
            {
                context.TenantInfos.Add(new TenantInfo() {  ConnectionString = "", Id=Guid.NewGuid().ToString(), Identifier="admin", Name="Admin Tenant"});
                context.SaveChanges();

                var tenantInfo = context.TenantInfos.FirstOrDefault();

                context.Tenants.Add(new Tenant
                {
                    ApplicationTitle = "Admin Tenant",
                    CreatedDate = DateTime.Now,
                    TenantId = tenantInfo.Id,
                    //IsEnabled = true,
                    //Subscription = (int)TenantSubscription.Pro,
                    //SubscriptionExipreDate = DateTime.Now.AddYears(100),
                    HostName = "tenant1.maa.com",
                    Name = "Admin"
                });

                context.SaveChanges();

                context.Roles.Add(new TenantRole { Name = "CentralAdmin", NormalizedName = "CENTRALADMIN" });
                context.SaveChanges();
            }
        }
    }
}
