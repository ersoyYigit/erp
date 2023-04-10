using ArdaManager.Application.Interfaces.Approval;
using ArdaManager.Application.Requests.Approval;
using ArdaManager.Application.Responses.Approval;
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
    public class ApprovalController : ControllerBase
    {
        private readonly IApprovalService _approvaelService;

        public ApprovalController(IApprovalService approvaelService)
        {
            _approvaelService = approvaelService;
        }



        [HttpPost("submit-for-approval")]
        public async Task<IActionResult> SubmitForApproval(SubmitApprovalRequest request)
        {
            return Ok(await _approvaelService.SubmitForApprovalAsync(request));
        }


        [HttpPost("approve-step")]
        public async Task<IActionResult> ApproveStep(ApproveRejectRequest request)
        {
            return Ok(await _approvaelService.ApproveStepAsync(request));
        }

        [HttpPost("reject-step")]
        public async Task<IActionResult> RejectStep(ApproveRejectRequest request)
        {
            return Ok(await _approvaelService.RejectStepAsync(request));
        }
        [HttpPost("update-docstate")]
        public async Task<IActionResult> UpdateDocState(UpdateDocStateRequest request)
        {
            return Ok(await _approvaelService.UpdateDocStateAsync(request));
        }

        [HttpGet("get-document-approval-status/{documentId:int}")]
        public async Task<IActionResult> GetDocumentApprovalStatus(int documentId)
        {
            return Ok(await _approvaelService.GetDocumentApprovalStatusAsync(documentId));
        }
        
        
        [HttpGet("get-document-final-status/{documentId:int}")]
        public async Task<IActionResult> GetDocumentFinalStatus(int documentId)
        {
            return Ok(await _approvaelService.GetDocumentFinalStatusAsync(documentId));
        }
        
        [HttpGet("get-waiting-statuses/{roleId}")]
        public async Task<IActionResult> GetWaitingStatusesByRole(string roleId)
        {
            return Ok(await _approvaelService.GetWaitingStatusesByRoleAsync(roleId));
        }
    }
}
