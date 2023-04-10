using ArdaManager.Application.Requests.Approval;
using ArdaManager.Application.Responses.Approval;
using ArdaManager.Client.Infrastructure.Extensions;
using ArdaManager.Client.Infrastructure.Managers.Approval.Scenario;
using ArdaManager.Client.Infrastructure.Routes;
using ArdaManager.Domain.Entities;
using ArdaManager.Domain.Entities.Approval;
using ArdaManager.Shared.Wrapper;
using Azure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Client.Infrastructure.Managers.Approval
{
    public class ApproveManager : IApproveManager
    {
        private readonly HttpClient _httpClient;

        public ApproveManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        public async Task<IResult<List<DocumentApprovalStatusResponse>>> GetDocumentApprovalStatusAsync(int documentId)
        {
            var response = await _httpClient.GetAsync(ApprovalEndpoints.GetDocumentApprovalStatus(documentId));
            var data = await response.ToResult<List<DocumentApprovalStatusResponse>>();
            return data;
        }
        public async Task<IResult<List<DocumentApprovalStatusResponse>>> GetWaitingStatusesByRoleAsync(string roleId)
        {
            var response = await _httpClient.GetAsync(ApprovalEndpoints.GetWaitingStatusesByRole(roleId));
            var data = await response.ToResult<List<DocumentApprovalStatusResponse>>();
            return data;
        }
        

        public async Task<IResult<BaseDoc>> GetDocumentFinalStatusAsync(int documentId)
        {
            var response = await _httpClient.GetAsync(ApprovalEndpoints.GetDocumentFinalStatus(documentId));
            var data = await response.ToResult<BaseDoc>();
            return data;
        }
        public async Task<IResult<BaseDoc>> SubmitForApprovalAsync(SubmitApprovalRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync(ApprovalEndpoints.SubmitForApproval,request);
            var data = await response.ToResult<BaseDoc>();
            return data;
        }


        public async Task<IResult> ApproveStepAsync(ApproveRejectRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync(ApprovalEndpoints.ApproveStep, request);
            var data = await response.ToResult();
            return data;
        }

        public async Task<IResult> RejectStepAsync(ApproveRejectRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync(ApprovalEndpoints.RejectStep, request);
            var data = await response.ToResult();
            return data;
        }
        public async Task<IResult> UpdateDocStateAsync(UpdateDocStateRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync(ApprovalEndpoints.UpdateDocState, request);
            var data = await response.ToResult();
            return data;
        }
        

    }
}
