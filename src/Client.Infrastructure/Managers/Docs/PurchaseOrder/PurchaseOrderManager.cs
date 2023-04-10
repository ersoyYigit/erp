using ArdaManager.Application.Features.Docs.PurchaseOrders.Commands;
using ArdaManager.Application.Features.Docs.PurchaseOrders.Queries;
using ArdaManager.Client.Infrastructure.Extensions;
using ArdaManager.Client.Infrastructure.Routes.Docs;
using ArdaManager.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Client.Infrastructure.Managers.Docs.PurchaseOrder
{
    public class PurchaseOrderManager : IPurchaseOrderManager
    {
        private readonly HttpClient _httpClient;

        public PurchaseOrderManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{PurchaseOrdersEndpoints.Delete}/{id}");
            return await response.ToResult<int>();
        }

        public async Task<IResult<List<PurchaseOrderResponse>>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync(PurchaseOrdersEndpoints.GetAll);
            return await response.ToResult<List<PurchaseOrderResponse>>();
        }

        public async Task<IResult<int>> SaveAsync(UpsertPurchaseOrderCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(PurchaseOrdersEndpoints.Save, request);
            return await response.ToResult<int>();
        }
    }
}
