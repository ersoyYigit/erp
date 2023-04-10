using ArdaManager.Application.Features.Molds.Commands.AddEdit;
using ArdaManager.Application.Features.Molds.Commands.Delete;
using ArdaManager.Application.Features.Molds.Queries.GetAllPaged;
using ArdaManager.Application.Features.Molds.Queries.GetMoldImage;
using ArdaManager.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Radzen.Blazor.Rendering;
using System.Threading.Tasks;
using Path = System.IO.Path;

namespace ArdaManager.Server.Controllers.v1.Catalog
{
    public class MoldsController : BaseApiController<MoldsController>
    {
        /// <summary>
        /// Get All Molds
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchString"></param>
        /// <param name="orderBy"></param>
        /// <param name="isMold"></param>
        /// <returns>Status 200 OK</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll(int pageNumber, int pageSize, string searchString,  string orderBy = null)
        {
            var products = await _mediator.Send(new GetAllPagedMoldsQuery(pageNumber, pageSize, searchString, orderBy));
            return Ok(products);
        }

        /// <summary>
        /// Get a Mold Image by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Molds.View)]
        [HttpGet("image/{id}")]
        public async Task<IActionResult> GetMoldImageAsync(int id)
        {
            var result = await _mediator.Send(new GetMoldImageQuery(id));
            //if (result.Succeeded)
                //result.Data = Path.Combine("D:\\Dev\\ArdaManager\\src\\Server", result.Data);
            return Ok(result);
        }

        /// <summary>
        /// Add/Edit a Mold
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Molds.Create)]
        [HttpPost]
        public async Task<IActionResult> Post(AddEditMoldCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        /// <summary>
        /// Delete a Mold
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK response</returns>
        [Authorize(Policy = Permissions.Molds.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _mediator.Send(new DeleteMoldCommand { Id = id }));
        }


    }
}