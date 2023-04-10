using ArdaManager.Application.Features.Docs.PurchaseOffers.Commands;
using ArdaManager.Application.Features.Docs.PurchaseOffers.Queries;
using ArdaManager.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Polly;
using System.Threading.Tasks;

namespace ArdaManager.Server.Controllers.v1.Purcheasing
{
    public class PurchaseOffersController : BaseApiController<PurchaseOffersController>
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var purchaseOffers = await _mediator.Send(new GetAllPurchaseOffersQuery());
            return Ok(purchaseOffers);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var purchaseOffer = await _mediator.Send(new GetPurchaseOfferByIdQuery { Id = id });
            return Ok(purchaseOffer);
        }

        [Authorize(Policy = Permissions.PurchaseOffers.Create)]
        [HttpPost]
        public async Task<IActionResult> Post(UpsertPurchaseOfferCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [Authorize(Policy = Permissions.PurchaseOffers.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _mediator.Send(new DeletePurchaseOfferCommand { Id = id }));
        }



        [HttpPost("choose")]
        public async Task<IActionResult> ChooseOffer(ChoosePurchaseOfferCommand command)
        {
            return Ok(await _mediator.Send(new ChoosePurchaseOfferCommand { Id = command.Id }));
        }
    }
}
