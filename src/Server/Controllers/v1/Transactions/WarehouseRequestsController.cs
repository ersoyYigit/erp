using ArdaManager.Application.Features.Docs.WarehouseRequests.Commands;
using ArdaManager.Application.Features.Docs.WarehouseRequests.Queries;
using ArdaManager.Server.Controllers.v1.Purcheasing;
using ArdaManager.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Polly;
using System.Threading.Tasks;

namespace ArdaManager.Server.Controllers.v1.Transactions
{
    public class WarehouseRequestsController : BaseApiController<WarehouseRequestsController>
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var purchaseOffers = await _mediator.Send(new GetAllWarehouseRequestsQuery());
            return Ok(purchaseOffers);
        }

        /*
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var purchaseOffer = await _mediator.Send(new GetWarehouseRequestByIdQuery { Id = id });
            return Ok(purchaseOffer);
        }*/

        [Authorize(Policy = Permissions.WarehouseRequests.Create)]
        [HttpPost]
        public async Task<IActionResult> Post(UpsertWarehouseRequestCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [Authorize(Policy = Permissions.WarehouseRequests.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _mediator.Send(new DeleteWarehouseRequestCommand { Id = id }));
        }


        /*
        [HttpPost("choose")]
        public async Task<IActionResult> ChooseOffer(ChooseWarehouseRequestCommand command)
        {
            return Ok(await _mediator.Send(new ChooseWarehouseRequestCommand { Id = command.Id }));
        }
        */
    }
}
