using AutoMapper;
using MAA.ActionTracking.Data.Entities;
using MAA.ActionTracking.Web.Models.Tenant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAA.ActionTracking.Web.Infrastructure.Mappers
{
    public class TenantMapperProfile : Profile
    {
        public TenantMapperProfile()
        {
            CreateMap<Tenant, TenantViewModel>();
            CreateMap<TenantViewModel, Tenant>();
        }
    }
}
