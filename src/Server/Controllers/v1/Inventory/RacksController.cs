using ArdaManager.Application.Features.Racks.Queries.GetAll;
using ArdaManager.Application.Features.Racks.Commands.AddEdit;
using ArdaManager.Application.Features.Racks.Commands.Delete;
using ArdaManager.Application.Features.Racks.Queries.GetById;
using ArdaManager.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ArdaManager.Server.Controllers.v1.Inventory
{
    public class RacksController : BaseApiController<RacksController>
    {
        /// <summary>
        /// Get All Racks
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var productRacks = await _mediator.Send(new GetAllRacksQuery());
            return Ok(productRacks);
        }

        /// <summary>
        /// Get a  Rack By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 Ok</returns>
        [Authorize(Policy = Permissions.Warehouses.View)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var productRack = await _mediator.Send(new GetRackByIdQuery() { Id = id });
            return Ok(productRack);
        }

        /// <summary>
        /// Get a Rack By Type
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 Ok</returns>
        [Authorize(Policy = Permissions.Warehouses.View)]
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetByType(int id)
        {
            var productRacks = await _mediator.Send(new GetRackByIdQuery() { Id = id });
            return Ok(productRacks);
        }




        /// <summary>
        /// Create/Update a  Rack
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Warehouses.Create)]
        [HttpPost]
        public async Task<IActionResult> Post(AddEditRackCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        /// <summary>
        /// Delete a  Rack
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Warehouses.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _mediator.Send(new DeleteRackCommand { Id = id }));
        }


    }
}
