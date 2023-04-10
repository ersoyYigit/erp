using ArdaManager.Application.Responses.Approval;
using ArdaManager.Client.Infrastructure.Extensions;
using ArdaManager.Client.Infrastructure.Routes;
using ArdaManager.Domain.Entities.Approval;
using ArdaManager.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Client.Infrastructure.Managers.Approval.Scenario
{
    public class ScenarioManager : IScenarioManager
    {
        private readonly HttpClient _httpClient;

        public ScenarioManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<List<ApprovalScenarioResponse>>> GetAllScenariosAsync()
        {
            var response = await _httpClient.GetAsync(ApprovalScenarioEndPoints.GetAllScenarios);
            var data = await response.ToResult<List<ApprovalScenarioResponse>>();
            return data;
        }

        public async Task<IResult<ApprovalScenarioResponse>> GetScenarioByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync(ApprovalScenarioEndPoints.GetScenarioById(id));
            var data = await response.ToResult<ApprovalScenarioResponse>();
            return data;
        }

        public async Task<IResult> UpsertScenarioAsync(ApprovalScenarioResponse approvalScenario)
        {
            var response = await _httpClient.PostAsJsonAsync(ApprovalScenarioEndPoints.UpsertScenario, approvalScenario);
            var data = await response.ToResult();
            return data;
        }

        

        public async Task<IResult> DeleteScenarioAsync(int id)
        {
            var response = await _httpClient.DeleteAsync(ApprovalScenarioEndPoints.DeleteScenario(id));
            var data = await response.ToResult();
            return data;
        }
    }
}