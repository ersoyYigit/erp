
using ArdaManager.Application.Features.Docs.WarehouseRequests.Commands;
using ArdaManager.Application.Features.Docs.WarehouseRequests.Queries;
using ArdaManager.Client.Infrastructure.Extensions;
using ArdaManager.Client.Infrastructure.Routes.Docs;
using ArdaManager.Domain.Enums.Doc;
using ArdaManager.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Client.Infrastructure.Managers.Docs.WarehouseRequest
{
    public class WarehouseRequestManager : IWarehouseRequestManager
    {
        private readonly HttpClient _httpClient;

        public WarehouseRequestManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }



        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{WarehouseRequestsEndpoints.Delete}/{id}");
            return await response.ToResult<int>();
        }

        


        public async Task<IResult<List<WarehouseRequestResponse>>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync(WarehouseRequestsEndpoints.GetAll);
            return await response.ToResult<List<WarehouseRequestResponse>>();
        }




        public async Task<IResult<int>> SaveAsync(UpsertWarehouseRequestCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(WarehouseRequestsEndpoints.Save, request);
            return await response.ToResult<int>();
        }
    }
}
