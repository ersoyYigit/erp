using ArdaManager.Application.Interfaces.Approval;
using ArdaManager.Application.Interfaces.Services;
using ArdaManager.Application.Responses.Approval;
using ArdaManager.Domain.Entities.Approval;
using ArdaManager.Infrastructure.Services;
using ArdaManager.Server.Services;
using ArdaManager.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Polly;
using System.Threading.Tasks;

namespace ArdaManager.Server.Controllers.Approval
{

    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = false)]
    public class ApprovalScenarioController : ControllerBase
    {
        private readonly IApprovalScenarioService _scenarioService;

        public ApprovalScenarioController(IApprovalScenarioService scenarioService)
        {
            _scenarioService = scenarioService;
        }




        /// <summary>
        /// Get user notifications
        /// </summary>
        /// <returns>Status 200 OK</returns>
        //Get user wise chat history
        [HttpGet("GetAllScenariosAsync")]
        public async Task<IActionResult> GetAllScenarios()
        {
            return Ok(await _scenarioService.GetAllAsync());
        }


        [HttpGet("GetScenarioByIdAsync/{id:int}")]
        public async Task<IActionResult> GetScenarioById(int id)
        {
            return Ok(await _scenarioService.GetByIdAsync(id));
        }

        [HttpPost("UpsertScenarioAsync")]
        public async Task<IActionResult> UpsertScenario(ApprovalScenarioResponse approvalScenario)
        {
            return Ok(await _scenarioService.UpsertAsync(approvalScenario));
        }

        
        [HttpDelete("DeleteScenarioAsync/{id:int}")]
        public async Task<IActionResult> DeleteScenario(int id)
        {
            return Ok(await _scenarioService.DeleteAsync(id));
        }
    }
}
