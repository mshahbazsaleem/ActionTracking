using MAA.ActionTracking.Data.Contexts;
using MAA.ActionTracking.Data.Entities;
using MAA.ActionTracking.Web.Infrastructure.Mappers;
using MAA.ActionTracking.Web.Models;
using MAA.ActionTracking.Web.Models.Tenant;
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
    public class TenantService : BaseService
    {
        public TenantService(ActionTrackingDbContext dbContext, IOptionsSnapshot<AppSettings> appSetting, HttpContext httpContext, ILogger<TenantService> logger) : base(dbContext, appSetting, httpContext, logger)
        {

        }

        public async Task<Tenant> GetModel()
        {
            var tenant = new Tenant().ToModel();

            return await Task.FromResult(tenant);
        }
        public async Task<JsonResult> GetAll(GridViewModel model)
        {
            var total = DbContext.Tenants.Count();
            var results = DbContext.Tenants.Skip((model.Page - 1) * model.Length).Take(model.Length).ToList().ToModel();

            return await Task.FromResult(new JsonResult(new GridDataViewModel<TenantViewModel>
            {
                draw = model.Draw,
                data = results,
                recordsFiltered = total,
                recordsTotal = total
            }));

        }

        public async Task<TenantViewModel> GetById(int id)
        {
            return await Task.FromResult(DbContext.Tenants.FirstOrDefault(t => t.Id.Equals(id)).ToModel());
        }


        public async Task<ResultViewModel> Save(TenantViewModel model)
        {
            var result = new ResultViewModel();
            try
            {
                if (model.Id.Equals(0))
                    DbContext.Tenants.Update(model.ToEntity());
                else
                    DbContext.Tenants.Add(model.ToEntity());

                await DbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                result.Message = ex.Message;
            }

            return await Task.FromResult(result);
        }

        public async Task<ResultViewModel> Delete(int id)
        {
            var result = new ResultViewModel();
            try
            {

                DbContext.Tenants.Remove(DbContext.Tenants.Single(t => t.Id == id));

                await DbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }

            return await Task.FromResult(result);
        }
    }
}
