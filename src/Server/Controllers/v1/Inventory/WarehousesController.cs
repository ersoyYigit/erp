using ArdaManager.Application.Features.Warehouses.Commands.AddEdit;
using ArdaManager.Application.Features.Warehouses.Commands.Delete;
using ArdaManager.Application.Features.Warehouses.Queries.GetAll;
using ArdaManager.Application.Features.Warehouses.Queries.GetById;
using ArdaManager.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ArdaManager.Server.Controllers.v1.Inventory
{
    public class WarehousesController : BaseApiController<WarehousesController>
    {
        /// <summary>
        /// Get All Warehouses
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var productWarehouses = await _mediator.Send(new GetAllWarehousesQuery());
            return Ok(productWarehouses);
        }

        /// <summary>
        /// Get a  Warehouse By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 Ok</returns>
        [Authorize(Policy = Permissions.Warehouses.View)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var productWarehouse = await _mediator.Send(new GetWarehouseByIdQuery() { Id = id });
            return Ok(productWarehouse);
        }

        /// <summary>
        /// Get a Warehouse By Type
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 Ok</returns>
        [Authorize(Policy = Permissions.Warehouses.View)]
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetByType(int id)
        {
            var productWarehouses = await _mediator.Send(new GetWarehouseByIdQuery() { Id = id });
            return Ok(productWarehouses);
        }




        /// <summary>
        /// Create/Update a  Warehouse
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Warehouses.Create)]
        [HttpPost]
        public async Task<IActionResult> Post(AddEditWarehouseCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        /// <summary>
        /// Delete a  Warehouse
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Warehouses.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _mediator.Send(new DeleteWarehouseCommand { Id = id }));
        }


    }
}
