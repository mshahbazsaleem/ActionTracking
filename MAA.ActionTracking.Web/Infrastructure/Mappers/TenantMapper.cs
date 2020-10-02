using AutoMapper;
using MAA.ActionTracking.Data.Entities;
using MAA.ActionTracking.Web.Models.Tenant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAA.ActionTracking.Web.Infrastructure.Mappers
{
    public static class TenantMapper
    {
        static TenantMapper()
        {
            Mapper = new MapperConfiguration(cfg => cfg.AddProfile<TenantMapperProfile>())
           .CreateMapper();
        }
        public static IMapper Mapper { get; }
        public static Tenant ToEntity(this TenantViewModel tenantViewModel)
        {
            return Mapper.Map<Tenant>(tenantViewModel);
        }
        public static TenantViewModel ToModel(this Tenant tenantViewModel)
        {
            return Mapper.Map<TenantViewModel>(tenantViewModel);
        }

        public static List<TenantViewModel> ToModel(this List<Tenant> tenantViewModel)
        {
            return Mapper.Map<List<TenantViewModel>>(tenantViewModel);
        }

    }
}
