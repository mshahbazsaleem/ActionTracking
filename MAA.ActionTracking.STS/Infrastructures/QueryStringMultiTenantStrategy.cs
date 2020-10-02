using Finbuckle.MultiTenant;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAA.ActionTracking.STS.Infrastructures
{
    public class QueryStringMultiTenantStrategy : IMultiTenantStrategy
    {
        private readonly HttpContext _context;
        public QueryStringMultiTenantStrategy(IHttpContextAccessor context)
        {
            _context = context.HttpContext;
        }

        public Task<string> GetIdentifierAsync(object context)
        {
            return Task.FromResult( "admin");
        }

    }
}
