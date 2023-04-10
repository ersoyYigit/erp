using ArdaManager.Application.Responses.Approval;
using ArdaManager.Domain.Entities.Approval;
using ArdaManager.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Client.Infrastructure.Managers.Approval.Scenario
{
    public interface IScenarioManager : IManager
    {
        Task<IResult<List<ApprovalScenarioResponse>>> GetAllScenariosAsync();
        Task<IResult<ApprovalScenarioResponse>> GetScenarioByIdAsync(int id);
        Task<IResult> UpsertScenarioAsync(ApprovalScenarioResponse approvalScenario);
        Task<IResult> DeleteScenarioAsync(int id);
    }
}
