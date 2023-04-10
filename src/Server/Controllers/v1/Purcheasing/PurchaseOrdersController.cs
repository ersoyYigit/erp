using ArdaManager.Application.Features.Docs.PurchaseOffers.Queries;
using ArdaManager.Application.Features.Docs.PurchaseOrders.Commands;
using ArdaManager.Application.Features.Docs.PurchaseOrders.Queries;
using ArdaManager.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Polly;
using System.Threading.Tasks;

namespace ArdaManager.Server.Controllers.v1.Purcheasing
{
    public class PurchaseOrdersController : BaseApiController<PurchaseOrdersController>
    {
        
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var purchaseOrders = await _mediator.Send(new GetAllPurchaseOrdersQuery());
            return Ok(purchaseOrders);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var purchaseOrder = await _mediator.Send(new GetPurchaseOrderByIdQuery { Id = id });
            return Ok(purchaseOrder);
        }

        [Authorize(Policy = Permissions.PurchaseOrders.Create)]
        [HttpPost]
        public async Task<IActionResult> Post(UpsertPurchaseOrderCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [Authorize(Policy = Permissions.PurchaseOrders.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _mediator.Send(new DeletePurchaseOrderCommand { Id = id }));
        }
    }
}
