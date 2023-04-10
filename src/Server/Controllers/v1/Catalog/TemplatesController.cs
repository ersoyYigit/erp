using ArdaManager.Application.Features.Templates.Commands.AddEdit;
using ArdaManager.Application.Features.Templates.Commands.Delete;
using ArdaManager.Application.Features.Templates.Queries.GetAllPaged;
using ArdaManager.Application.Features.Templates.Queries.GetTemplateImage;
using ArdaManager.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Radzen.Blazor.Rendering;
using System.Threading.Tasks;
using Path = System.IO.Path;

namespace ArdaManager.Server.Controllers.v1.Catalog
{
    public class TemplatesController : BaseApiController<TemplatesController>
    {
        /// <summary>
        /// Get All Templates
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchString"></param>
        /// <param name="orderBy"></param>
        /// <param name="isTemplate"></param>
        /// <returns>Status 200 OK</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll(int pageNumber, int pageSize, string searchString,  string orderBy = null)
        {
            var products = await _mediator.Send(new GetAllPagedTemplatesQuery(pageNumber, pageSize, searchString, orderBy));
            return Ok(products);
        }

        /// <summary>
        /// Get a Template Image by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Templates.View)]
        [HttpGet("image/{id}")]
        public async Task<IActionResult> GetTemplateImageAsync(int id)
        {
            var result = await _mediator.Send(new GetTemplateImageQuery(id));
            //if (result.Succeeded)
                //result.Data = Path.Combine("D:\\Dev\\ArdaManager\\src\\Server", result.Data);
            return Ok(result);
        }

        /// <summary>
        /// Add/Edit a Template
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Templates.Create)]
        [HttpPost]
        public async Task<IActionResult> Post(AddEditTemplateCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        /// <summary>
        /// Delete a Template
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK response</returns>
        [Authorize(Policy = Permissions.Templates.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _mediator.Send(new DeleteTemplateCommand { Id = id }));
        }


    }
}