using AutoMapper;
using MAA.ActionTracking.Data.Entities;
using MAA.ActionTracking.Web.Models.SystemVariable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAA.ActionTracking.Web.Infrastructure.Mappers
{
    public class SystemVariableMapperProfile : Profile
    {
        public SystemVariableMapperProfile()
        {
            CreateMap<SystemVariable, SystemVariableViewModel>();
            CreateMap<SystemVariableViewModel, SystemVariable>();
        }
    }
}
