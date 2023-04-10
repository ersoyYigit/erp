using ArdaManager.Application.Features.Categories.Commands.AddEdit;
using ArdaManager.Application.Features.Categories.Commands.Delete;
using ArdaManager.Application.Features.Categories.Queries.GetAll;
using ArdaManager.Application.Features.Categories.Queries.GetById;
using ArdaManager.Application.Features.Categories.Queries.GetCategoriesByType;
using ArdaManager.Domain.Enums;
using ArdaManager.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ArdaManager.Server.Controllers.v1.Misc
{

    
    public class CategoriesController : BaseApiController<CategoriesController>
    {
        /// <summary>
        /// Get All ProductCategories
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var productCategories = await _mediator.Send(new GetAllCategoriesQuery());
            return Ok(productCategories);
        }

        /// <summary>
        /// Get a ProductCategory By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 Ok</returns>
        [Authorize(Policy = Permissions.Categories.View)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var productCategory = await _mediator.Send(new GetCategoryByIdQuery() { Id = id });
            return Ok(productCategory);
        }

        /// <summary>
        /// Get a ProductCategory By Type
        /// </summary>
        /// <param name="type"></param>
        /// <returns>Status 200 Ok</returns>
        [Authorize(Policy = Permissions.Categories.View)]
        [HttpGet("GetByType/{type}")]
        public async Task<IActionResult> GetByType(int type)
        {
            var productCategories = await _mediator.Send(new GetCategoriesByTypeQuery() { Type = type });
            return Ok(productCategories);
        }




        /// <summary>
        /// Create/Update a ProductCategory
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        [HttpPost]
        public async Task<IActionResult> Post(AddEditCategoryCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        /// <summary>
        /// Delete a ProductCategory
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Categories.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _mediator.Send(new DeleteCategoryCommand { Id = id }));
        }
        
        
    }
    
}
