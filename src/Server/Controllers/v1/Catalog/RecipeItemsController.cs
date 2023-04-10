using ArdaManager.Application.Features.RecipeItems.Commands.AddEdit;
using ArdaManager.Application.Features.RecipeItems.Commands.Delete;
using ArdaManager.Application.Features.RecipeItems.Queries.GetByOwnerProductId;
using ArdaManager.Server.Controllers.v1.Misc;
using ArdaManager.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Polly;
using System.Threading.Tasks;

namespace ArdaManager.Server.Controllers.v1.Catalog
{
    public class RecipeItemsController : BaseApiController<RecipeItemsController>
    {
        /*
        /// <summary>
        /// Get All ProductRecipeItems
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.RecipeItems.View)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var productRecipeItems = await _mediator.Send(new GetAllRecipeItemsQuery());
            return Ok(productRecipeItems);
        }

        /// <summary>
        /// Get a ProductRecipeItem By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 Ok</returns>
        [Authorize(Policy = Permissions.RecipeItems.View)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var productRecipeItem = await _mediator.Send(new GetRecipeItemByIdQuery() { Id = id });
            return Ok(productRecipeItem);
        }
        */
        /// <summary>
        /// Get a ProductRecipeItem By Type
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 Ok</returns>
        [HttpGet("GetByOwnerProductId/{id}")]
        public async Task<IActionResult> GetByOwnerProductId(int id)
        {
            var productRecipeItems = await _mediator.Send(new GetRecipeItemsByOwnerProductIdQuery() { OwnerProductId = id });
            return Ok(productRecipeItems);
        }




        /// <summary>
        /// Create/Update a ProductRecipeItem
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.RecipeItems.Create)]
        [HttpPost]
        public async Task<IActionResult> Post(AddEditRecipeItemCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        /// <summary>
        /// Delete a ProductRecipeItem
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.RecipeItems.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _mediator.Send(new DeleteRecipeItemCommand { Id = id }));
        }
    }
}
