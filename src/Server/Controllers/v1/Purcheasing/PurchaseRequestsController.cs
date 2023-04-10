using ArdaManager.Application.Features.Docs.PurchaseRequests.Commands.AddEdit;
using ArdaManager.Application.Features.Docs.PurchaseRequests.Commands.Delete;
using ArdaManager.Application.Features.Docs.PurchaseRequests.Queries.GetAll;
using ArdaManager.Application.Features.Docs.PurchaseRequests.Queries.GetById;
using ArdaManager.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Polly;
using System.Threading.Tasks;

namespace ArdaManager.Server.Controllers.v1.Purcheasing
{
    public class PurchaseRequestsController : BaseApiController<PurchaseRequestsController>
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var purchaseRequests = await _mediator.Send(new GetAllPurchaseRequestsQuery());
            return Ok(purchaseRequests);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var purchaseRequest = await _mediator.Send(new GetPurchaseRequestByIdQuery() { Id = id });
            return Ok(purchaseRequest);
        }

        [Authorize(Policy = Permissions.PurchaseRequests.Create)]
        [HttpPost]
        public async Task<IActionResult> Post(AddEditPurchaseRequestCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [Authorize(Policy = Permissions.PurchaseRequests.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _mediator.Send(new DeletePurchaseRequestCommand { Id = id }));
        }
    }
}