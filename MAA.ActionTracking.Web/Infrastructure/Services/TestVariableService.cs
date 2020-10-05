using MAA.ActionTracking.Data.Contexts;
using MAA.ActionTracking.Data.Entities;
using MAA.ActionTracking.Web.Infrastructure.Mappers;
using MAA.ActionTracking.Web.Models;
using MAA.ActionTracking.Web.Models.TestVariable;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MAA.ActionTracking.Web.Infrastructure.Services
{
    public class TestVariableService : BaseService
    {
        public TestVariableService(ActionTrackingDbContext dbContext, IOptionsSnapshot<AppSettings> appSetting, HttpContext httpContext, ILogger<TestVariableService> logger) : base(dbContext, appSetting, httpContext, logger)
        {

        }

        public async Task<TestVariable> GetModel()
        {
            return await Task.FromResult(new TestVariable());
        }
        public async Task<JsonResult> GetAll(GridViewModel model)
        {
            var total = DbContext.TestVariables.Count();
            var results = DbContext.TestVariables.Skip((model.Page-1) * model.Length).Take(model.Length).ToList().ToModel();

            return await Task.FromResult(new JsonResult(new GridDataViewModel<TestVariableViewModel>
            {
                draw = model.Draw,
                data = results,
                recordsFiltered = total,
                recordsTotal = total
            }));

        }

        public async Task<TestVariableViewModel> GetById(int id)
        {
            return await Task.FromResult(DbContext.TestVariables.FirstOrDefault(t => t.Id.Equals(id)).ToModel());
        }


        public async Task<ResultViewModel> Save(TestVariableViewModel model)
        {
            var result = new ResultViewModel();
            try
            {
                if (model.Id.Equals(0))
                    DbContext.TestVariables.Update(model.ToEntity());
                else
                    DbContext.TestVariables.Add(model.ToEntity());

                await DbContext.SaveChangesAsync();
            }
            catch (Exception ex) {

                result.Message = ex.Message;
            }

            return await Task.FromResult(result);
        }

        public async Task<ResultViewModel> Delete(int id)
        {
            var result = new ResultViewModel();
            try
            {

                DbContext.TestVariables.Remove(DbContext.TestVariables.Single(t => t.Id == id));

                await DbContext.SaveChangesAsync();
            }
            catch (Exception ex) {
                result.Message = ex.Message;
            }

            return await Task.FromResult(result);
        }
    }
}
