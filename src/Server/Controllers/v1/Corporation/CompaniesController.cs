using ArdaManager.Application.Features.Companies.Commands.AddEdit;
using ArdaManager.Application.Features.Companies.Commands.Delete;
using ArdaManager.Application.Features.Companies.Queries.Export;
using ArdaManager.Application.Features.Companies.Queries.GetAllPaged;
using ArdaManager.Application.Features.Companies.Queries.Search;
using ArdaManager.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ArdaManager.Server.Controllers.v1.Corporation
{

    public class CompaniesController : BaseApiController<CompaniesController>
    {
        /// <summary>
        /// Get All Companies
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchString"></param>
        /// <param name="orderBy"></param>
        /// <returns>Status 200 OK</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll(int pageNumber, int pageSize, string searchString, string orderBy = null)
        {
            var companies = await _mediator.Send(new GetAllCompaniesQuery(pageNumber, pageSize, searchString, orderBy));
            return Ok(companies);
        }



        /// <summary>
        /// Add/Edit a Company
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Companies.Create)]
        [HttpPost]
        public async Task<IActionResult> Post(AddEditCompanyCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        /// <summary>
        /// Delete a Company
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK response</returns>
        [Authorize(Policy = Permissions.Companies.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _mediator.Send(new DeleteCompanyCommand { Id = id }));
        }

        /// <summary>
        /// Search Companies and Export to Excel
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Companies.Export)]
        [HttpGet("export")]
        public async Task<IActionResult> Export(string searchString = "")
        {
            return Ok(await _mediator.Send(new ExportCompaniesQuery(searchString)));
        }



        [HttpGet("filtered")]
        public async Task<IActionResult> GetFiltered(CompanySearchQuery query)
        {
            var products = await _mediator.Send(query);
            return Ok(products);
        }

        [HttpPost("filtered-post")]
        public async Task<IActionResult> GetFilteredPost(CompanySearchQuery query)
        {
            var products = await _mediator.Send(query);
            return Ok(products);
        }
    }
}
