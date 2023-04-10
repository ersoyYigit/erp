using ArdaManager.Application.Features.Docs.PurchaseRequests.Commands.AddEdit;
using ArdaManager.Application.Features.Docs.PurchaseRequests.Queries;
using ArdaManager.Client.Infrastructure.Extensions;
using ArdaManager.Client.Infrastructure.Managers.Docs.PurcheaseRequest;
using ArdaManager.Client.Infrastructure.Routes.Docs;
using ArdaManager.Shared.Wrapper;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ArdaManager.Client.Infrastructure.Managers.Docs.PurchaseRequest
{
    public class PurchaseRequestManager : IPurchaseRequestManager
    {
        private readonly HttpClient _httpClient;

        public PurchaseRequestManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{PurchaseRequestsEndpoints.Delete}/{id}");
            return await response.ToResult<int>();
        }

        public async Task<IResult<List<PurchaseRequestDto>>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync(PurchaseRequestsEndpoints.GetAll);
            return await response.ToResult<List<PurchaseRequestDto>>();
        }

        

        public async Task<IResult<int>> SaveAsync(AddEditPurchaseRequestCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(PurchaseRequestsEndpoints.Save, request);
            return await response.ToResult<int>();
        }
    }
}