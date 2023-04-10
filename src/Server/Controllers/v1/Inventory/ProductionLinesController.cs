using ArdaManager.Application.Features.ProductionLines.Commands.AddEdit;
using ArdaManager.Application.Features.ProductionLines.Commands.Delete;
using ArdaManager.Application.Features.ProductionLines.Queries.GetAll;
using ArdaManager.Application.Features.ProductionLines.Queries.GetById;
using ArdaManager.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ArdaManager.Server.Controllers.v1.Inventory
{
    public class ProductionLinesController : BaseApiController<ProductionLinesController>
    {
        /// <summary>
        /// Get All ProductionLines
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var productProductionLines = await _mediator.Send(new GetAllProductionLinesQuery());
            return Ok(productProductionLines);
        }

        /// <summary>
        /// Get a  ProductionLine By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 Ok</returns>
        [Authorize(Policy = Permissions.ProductionLines.View)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var productProductionLine = await _mediator.Send(new GetProductionLineByIdQuery() { Id = id });
            return Ok(productProductionLine);
        }

        /// <summary>
        /// Get a ProductionLine By Type
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 Ok</returns>
        [Authorize(Policy = Permissions.ProductionLines.View)]
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetByType(int id)
        {
            var productProductionLines = await _mediator.Send(new GetProductionLineByIdQuery() { Id = id });
            return Ok(productProductionLines);
        }




        /// <summary>
        /// Create/Update a  ProductionLine
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.ProductionLines.Create)]
        [HttpPost]
        public async Task<IActionResult> Post(AddEditProductionLineCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        /// <summary>
        /// Delete a  ProductionLine
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.ProductionLines.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _mediator.Send(new DeleteProductionLineCommand { Id = id }));
        }


    }
}
