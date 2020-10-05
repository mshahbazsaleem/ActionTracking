using MAA.ActionTracking.Data.Contexts;
using MAA.ActionTracking.Data.Entities;
using MAA.ActionTracking.Web.Infrastructure.Mappers;
using MAA.ActionTracking.Web.Models;
using MAA.ActionTracking.Web.Models.SystemVariable;
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
    public class SystemVariableService : BaseService
    {
        public SystemVariableService(ActionTrackingDbContext dbContext, IOptionsSnapshot<AppSettings> appSetting, HttpContext httpContext, ILogger<SystemVariableService> logger) : base(dbContext, appSetting, httpContext, logger)
        {

        }

        public async Task<SystemVariable> GetModel()
        {
            return await Task.FromResult(new SystemVariable());
        }
        public async Task<JsonResult> GetAll(GridViewModel model)
        {
            var total = DbContext.SystemVariables.Count();
            var results = DbContext.SystemVariables.Skip((model.Page-1) * model.Length).Take(model.Length).ToList().ToModel();

            return await Task.FromResult(new JsonResult(new GridDataViewModel<SystemVariableViewModel>
            {
                draw = model.Draw,
                data = results,
                recordsFiltered = total,
                recordsTotal = total
            }));

        }

        public async Task<SystemVariableViewModel> GetById(int id)
        {
            return await Task.FromResult(DbContext.SystemVariables.FirstOrDefault(t => t.Id.Equals(id)).ToModel());
        }


        public async Task<ResultViewModel> Save(SystemVariableViewModel model)
        {
            var result = new ResultViewModel();
            try
            {
                if (model.Id.Equals(0))
                    DbContext.SystemVariables.Update(model.ToEntity());
                else
                    DbContext.SystemVariables.Add(model.ToEntity());

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

                DbContext.SystemVariables.Remove(DbContext.SystemVariables.Single(t => t.Id == id));

                await DbContext.SaveChangesAsync();
            }
            catch (Exception ex) {
                result.Message = ex.Message;
            }

            return await Task.FromResult(result);
        }
    }
}
