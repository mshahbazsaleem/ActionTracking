using AutoMapper;
using MAA.ActionTracking.Data.Entities;
using MAA.ActionTracking.Web.Models.TestVariable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAA.ActionTracking.Web.Infrastructure.Mappers
{
    public class TestVariableMapperProfile : Profile
    {
        public TestVariableMapperProfile()
        {
            CreateMap<TestVariable, TestVariableViewModel>();
            CreateMap<TestVariableViewModel, TestVariable>();
        }
    }
}
