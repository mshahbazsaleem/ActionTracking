using AutoMapper;
using MAA.ActionTracking.Data.Entities;
using MAA.ActionTracking.Web.Models.TestVariable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAA.ActionTracking.Web.Infrastructure.Mappers
{
    public static class TestVariableMapper
    {
        static TestVariableMapper()
        {
            Mapper = new MapperConfiguration(cfg => cfg.AddProfile<TestVariableMapperProfile>())
           .CreateMapper();
        }
        public static IMapper Mapper { get; }
        public static TestVariable ToEntity(this TestVariableViewModel variableViewModel)
        {
            return Mapper.Map<TestVariable>(variableViewModel);
        }
        public static TestVariableViewModel ToModel(this TestVariable variableViewModel)
        {
            return Mapper.Map<TestVariableViewModel>(variableViewModel);
        }

        public static List<TestVariableViewModel> ToModel(this List<TestVariable> variableViewModel)
        {
            return Mapper.Map<List<TestVariableViewModel>>(variableViewModel);
        }

    }
}
