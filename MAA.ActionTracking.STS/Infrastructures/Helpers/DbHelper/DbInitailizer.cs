using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MAA.ActionTracking.Data.Contexts;
using System.Linq;

namespace MAA.ActionTracking.WebHost.Infrastructures.Helpers.DbHelper
{
    public static class DbInitailizer
    {
        public static void InitializeDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var systemDbContext = serviceScope.ServiceProvider.GetRequiredService<ActionTrackingDbContext>();
                systemDbContext.Database.Migrate();
            }
        }
    }
}
