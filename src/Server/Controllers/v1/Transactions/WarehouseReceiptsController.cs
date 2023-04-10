using ArdaManager.Application.Features.Docs.WarehouseReceipts.Queries.GetById;
using ArdaManager.Application.Features.Docs.WarehouseReceipts.Commands.AddEdit;
using ArdaManager.Application.Features.Docs.WarehouseReceipts.Commands.Delete;
using ArdaManager.Application.Features.Docs.WarehouseReceipts.Queries.GetAll;
using ArdaManager.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Polly;
using System.Threading.Tasks;
using ArdaManager.Domain.Enums.Doc;

namespace ArdaManager.Server.Controllers.v1.Transactions
{
    public class WarehouseReceiptsController : BaseApiController<WarehouseReceiptsController>
    {
        /// <summary>
        /// Get All WarehouseReceipts
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var templateWorkOrders = await _mediator.Send(new GetAllWarehouseReceiptsQuery());
            return Ok(templateWorkOrders);
        }        
        
        
        /// <summary>
        /// Get All WarehouseReceipts
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.WarehouseReceipts.View)]
        [HttpGet("GetAllByType/{type}/")]
        public async Task<IActionResult> GetAllByType(WarehouseReceiptType type)
        {
            var templateWorkOrders = await _mediator.Send(new GetAllWarehouseReceiptsQuery() { WarehouseReceiptType = type});
            return Ok(templateWorkOrders);
        }        
        
        
        
        /// <summary>
        /// Get All WarehouseReceipts
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.WarehouseReceipts.View)]
        [HttpGet("GetAllByWarehouseId/{type}/{id}")]
        public async Task<IActionResult> GetAllByWarehouseId(WarehouseReceiptType type, int id)
        {
            var templateWorkOrders = await _mediator.Send(new GetAllWarehouseReceiptsByWarehouseIdQuery() { WarehouseReceiptType = type, WarehouseId = id });
            return Ok(templateWorkOrders);
        }


        /// <summary>
        /// Get a WarehouseReceipt By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 Ok</returns>
        [Authorize(Policy = Permissions.WarehouseReceipts.View)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var templateWorkOrder = await _mediator.Send(new GetWarehouseReceiptByIdQuery() { Id = id });
            return Ok(templateWorkOrder);
        }



        /// <summary>
        /// Add/Edit a WarehouseReceipt
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.WarehouseReceipts.Create)]
        [HttpPost]
        public async Task<IActionResult> Post(AddEditWarehouseReceiptCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        /// <summary>
        /// Delete a WarehouseReceipt
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK response</returns>
        [Authorize(Policy = Permissions.WarehouseReceipts.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _mediator.Send(new DeleteWarehouseReceiptCommand { Id = id }));
        }
    }
}
