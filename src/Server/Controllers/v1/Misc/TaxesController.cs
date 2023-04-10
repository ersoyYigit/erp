using ArdaManager.Application.Features.Taxes.Commands;
using ArdaManager.Application.Features.Taxes.Queries;
using ArdaManager.Server.Controllers.v1.Catalog;
using ArdaManager.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ArdaManager.Server.Controllers.v1.Misc
{
    public class TaxesController : BaseApiController<TaxesController>
    {
        /// <summary>
        /// Get All Taxes
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var patterns = await _mediator.Send(new GetAllTaxesQuery());
            return Ok(patterns);
        }


        /// <summary>
        /// Create/Update a Tax
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Taxes.Create)]
        [HttpPost]
        public async Task<IActionResult> Post(UpsertTaxCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        /// <summary>
        /// Delete a Tax
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Taxes.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _mediator.Send(new DeleteTaxCommand { Id = id }));
        }
    }
}
