using ArdaManager.Application.Features.ProductCategories.Queries.GetAll;
using ArdaManager.Application.Features.ProductCategories.Queries.GetById;
using ArdaManager.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ArdaManager.Application.Features.ProductCategories.Commands.AddEdit;
using ArdaManager.Application.Features.ProductCategories.Commands.Delete;
using ArdaManager.Application.Features.ProductCategories.Queries.Export;

namespace ArdaManager.Server.Controllers.v1.Catalog
{
    public class ProductCategoriesController : BaseApiController<ProductCategoriesController>
    {
        /// <summary>
        /// Get All ProductCategories
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var productCategories = await _mediator.Send(new GetAllProductCategoriesQuery());
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
            var productCategory = await _mediator.Send(new GetProductCategoryByIdQuery() { Id = id });
            return Ok(productCategory);
        }

        /// <summary>
        /// Create/Update a ProductCategory
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Categories.Create)]
        [HttpPost]
        public async Task<IActionResult> Post(AddEditProductCategoryCommand command)
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
            return Ok(await _mediator.Send(new DeleteProductCategoryCommand { Id = id }));
        }

        /// <summary>
        /// Search ProductCategories and Export to Excel
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns></returns>
        [Authorize(Policy = Permissions.Categories.Export)]
        [HttpGet("export")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> Export(string searchString = "")
        {
            return Ok(await _mediator.Send(new ExportProductCategoriesQuery(searchString)));
        }
    }
}