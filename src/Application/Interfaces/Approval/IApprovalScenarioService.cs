using ArdaManager.Application.Responses.Approval;
using ArdaManager.Domain.Entities.Approval;
using ArdaManager.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Application.Interfaces.Approval
{
    public interface IApprovalScenarioService
    {
        Task<IResult<List<ApprovalScenarioResponse>>> GetAllAsync();
        Task<IResult<ApprovalScenarioResponse>> GetByIdAsync(int id);
        Task<IResult> UpsertAsync(ApprovalScenarioResponse approvalScenario);
        Task<IResult> DeleteAsync(int id);
    }
}
