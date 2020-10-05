using AutoMapper;
using MAA.ActionTracking.Data.Entities;
using MAA.ActionTracking.Web.Models.SystemVariable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAA.ActionTracking.Web.Infrastructure.Mappers
{
    public static class SystemVariableMapper
    {
        static SystemVariableMapper()
        {
            Mapper = new MapperConfiguration(cfg => cfg.AddProfile<SystemVariableMapperProfile>())
           .CreateMapper();
        }
        public static IMapper Mapper { get; }
        public static SystemVariable ToEntity(this SystemVariableViewModel variableViewModel)
        {
            return Mapper.Map<SystemVariable>(variableViewModel);
        }
        public static SystemVariableViewModel ToModel(this SystemVariable variableViewModel)
        {
            return Mapper.Map<SystemVariableViewModel>(variableViewModel);
        }

        public static List<SystemVariableViewModel> ToModel(this List<SystemVariable> variableViewModel)
        {
            return Mapper.Map<List<SystemVariableViewModel>>(variableViewModel);
        }

    }
}
