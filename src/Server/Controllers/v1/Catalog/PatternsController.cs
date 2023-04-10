using ArdaManager.Application.Features.Patterns.Queries.GetAll;
using ArdaManager.Application.Features.Patterns.Queries.GetById;
using ArdaManager.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ArdaManager.Application.Features.Patterns.Commands.AddEdit;
using ArdaManager.Application.Features.Patterns.Commands.Delete;
using ArdaManager.Application.Features.Patterns.Queries.Export;

namespace ArdaManager.Server.Controllers.v1.Catalog
{
    public class PatternsController : BaseApiController<PatternsController>
    {
        /// <summary>
        /// Get All Patterns
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var patterns = await _mediator.Send(new GetAllPatternsQuery());
            return Ok(patterns);
        }

        /// <summary>
        /// Get a Pattern By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 Ok</returns>
        [Authorize(Policy = Permissions.Patterns.View)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var pattern = await _mediator.Send(new GetPatternByIdQuery() { Id = id });
            return Ok(pattern);
        }

        /// <summary>
        /// Create/Update a Pattern
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Patterns.Create)]
        [HttpPost]
        public async Task<IActionResult> Post(AddEditPatternCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        /// <summary>
        /// Delete a Pattern
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Patterns.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _mediator.Send(new DeletePatternCommand { Id = id }));
        }

        /// <summary>
        /// Search Patterns and Export to Excel
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns></returns>
        [Authorize(Policy = Permissions.Patterns.Export)]
        [HttpGet("export")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> Export(string searchString = "")
        {
            return Ok(await _mediator.Send(new ExportPatternsQuery(searchString)));
        }
    }
}