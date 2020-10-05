using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MAA.ActionTracking.Web.Infrastructure.Services;
using MAA.ActionTracking.Web.Models;
using MAA.ActionTracking.Web.Models.Tenant;
using Microsoft.AspNetCore.Mvc;

namespace MAA.ActionTracking.Web.Controllers.Tenant
{
    [ApiController]
    [Route("api/tenant")]
    public class TenantAPIController :ControllerBase
    {
        private readonly TenantService _service;

        public TenantAPIController(TenantService service)
        {
            _service = service;
        }

        // GET api/tenant/list
        /// <summary>
        /// Retrieve Tenants.
        /// </summary>
        /// <returns>Tenants.</returns>
        /// <response code="200">Tenants successfully retrieved.</response>
        /// <response code="400">The request parameters were invalid or a timeout occurred while retrieving tenant .</response>
        [HttpPost("list")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<TenantsViewModel>> GetAll([FromBody] GridViewModel model)
        {
            return await _service.GetAll(model);
           
        }

      
        // GET api/tenant/1
        /// <summary>
        /// Retrieve tenant  based on primary key.
        /// </summary>
        /// <param name="id">Tenant Primary Key.</param>
        /// <returns>Tenants.</returns>
        /// <response code="200">Tenant successfully retrieved.</response>
        /// <response code="400">The request parameters were invalid or a timeout occurred while retrieving tenant .</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<TenantViewModel>> GetById(int id)
        {
            return Ok(await _service.GetById(id));
        }

        // POST api/tenant
        /// <summary>
        /// Create tenant 
        /// </summary>
        /// <param name="entity">Tenant  entity</param>
        /// <returns>Status.</returns>
        /// <response code="200">Tenant successfully created.</response>
        /// <response code="400">The request parameters were invalid or a timeout occurred while saving tenant .</response>
        [HttpPost]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> Post([FromBody] TenantViewModel model)
        {
            return Ok(await _service.Save(model));
        }

        // PUT api/tenant/5
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> Put(int id, [FromBody] TenantViewModel model)
        {
            model.Id = id;
            return Ok(await _service.Save(model));
        }

        // DELETE api/tenant/5
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> Delete(int id)
        {
            return Ok(await _service.Delete(id));
        }
    }
}
