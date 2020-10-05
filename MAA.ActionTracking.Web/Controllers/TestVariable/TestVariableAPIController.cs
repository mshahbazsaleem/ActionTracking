using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MAA.ActionTracking.Web.Infrastructure.Services;
using MAA.ActionTracking.Web.Models;
using MAA.ActionTracking.Web.Models.TestVariable;
using Microsoft.AspNetCore.Mvc;

namespace MAA.ActionTracking.Web.Controllers.TestVariable
{
    [ApiController]
    [Route("api/test")]
    public class TestVariableAPIController :ControllerBase
    {
        private readonly TestVariableService _service;

        public TestVariableAPIController(TestVariableService service)
        {
            _service = service;
        }

        // GET api/variable/list
        /// <summary>
        /// Retrieve TestVariables.
        /// </summary>
        /// <returns>TestVariables.</returns>
        /// <response code="200">TestVariables successfully retrieved.</response>
        /// <response code="400">The request parameters were invalid or a timeout occurred while retrieving variable .</response>
        [HttpPost("list")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<TestVariablesViewModel>> GetAll([FromBody] GridViewModel model)
        {
            return await _service.GetAll(model);
           
        }

      
        // GET api/variable/1
        /// <summary>
        /// Retrieve variable  based on primary key.
        /// </summary>
        /// <param name="id">TestVariable Primary Key.</param>
        /// <returns>TestVariables.</returns>
        /// <response code="200">TestVariable successfully retrieved.</response>
        /// <response code="400">The request parameters were invalid or a timeout occurred while retrieving variable .</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<TestVariableViewModel>> GetById(int id)
        {
            return Ok(await _service.GetById(id));
        }

        // POST api/variable
        /// <summary>
        /// Create variable 
        /// </summary>
        /// <param name="entity">TestVariable  entity</param>
        /// <returns>Status.</returns>
        /// <response code="200">TestVariable successfully created.</response>
        /// <response code="400">The request parameters were invalid or a timeout occurred while saving variable .</response>
        [HttpPost]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> Post([FromBody] TestVariableViewModel model)
        {
            return Ok(await _service.Save(model));
        }

        // PUT api/variable/5
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> Put(int id, [FromBody] TestVariableViewModel model)
        {
            model.Id = id;
            return Ok(await _service.Save(model));
        }

        // DELETE api/variable/5
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> Delete(int id)
        {
            return Ok(await _service.Delete(id));
        }
    }
}
