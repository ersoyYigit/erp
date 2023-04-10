using ArdaManager.Application.Features.CompanyFairs.Commands.AddEdit;
using ArdaManager.Application.Features.CompanyFairs.Commands.Delete;
using ArdaManager.Application.Features.CompanyFairs.Queries.GetAll;
using ArdaManager.Application.Features.CompanyFairs.Queries.GetByCompany;
using ArdaManager.Application.Features.CompanyFairs.Queries.GetById;
using ArdaManager.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ArdaManager.Server.Controllers.v1.Corporation
{
    public class CompanyFairsController : BaseApiController<FairsController>
    {
        /// <summary>
        /// Get All Fairs
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Companies.View)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var fairs = await _mediator.Send(new GetAllCompanyFairsQuery());
            return Ok(fairs);
        }

        /// <summary>
        /// Get a Fair By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 Ok</returns>
        [Authorize(Policy = Permissions.Companies.View)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var fair = await _mediator.Send(new GetCompanyFairByIdQuery() { Id = id });
            return Ok(fair);
        }

        /// <summary>
        /// Create/Update a Fair
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Companies.Create)]
        [HttpPost]
        public async Task<IActionResult> Post(AddEditCompanyFairCommand command)
        {
            return Ok(await _mediator.Send(command));
        }


        /// <summary>
        /// Get a Fairs By Company Id
        /// </summary>
        /// <param name="companyid"></param>
        /// <returns>Status 200 Ok</returns>
        [HttpGet("GetByCompanyId/{companyid}")]
        public async Task<IActionResult> GetByCompanyId(int companyid)
        {
            var companyfairs = await _mediator.Send(new GetCompanyFairsByCompanyIdQuery() { CompanyId = companyid });
            return Ok(companyfairs);
        }

        /// <summary>
        /// Delete a Fair
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Companies.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _mediator.Send(new DeleteCompanyFairCommand { Id = id }));
        }

        
    }
}
