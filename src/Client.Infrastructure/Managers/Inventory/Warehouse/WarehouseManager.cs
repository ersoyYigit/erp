using ArdaManager.Application.Features.Patterns.Queries.GetAll;
using ArdaManager.Client.Infrastructure.Extensions;
using ArdaManager.Shared.Wrapper;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using ArdaManager.Application.Features.Patterns.Commands.AddEdit;
using ArdaManager.Application.Features.Warehouses.Queries.GetAll;
using ArdaManager.Application.Features.Warehouses.Commands.AddEdit;
//using ArdaManager.Application.Features.Patterns.Commands.Import;


namespace ArdaManager.Client.Infrastructure.Managers.Inventory.Warehouse
{
    public class WarehouseManager : IWarehouseManager
    {
        private readonly HttpClient _httpClient;

        public WarehouseManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }



        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{Routes.WarehousesEndpoints.Delete}/{id}");
            return await response.ToResult<int>();
        }

        public async Task<IResult<List<GetAllWarehousesResponse>>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync(Routes.WarehousesEndpoints.GetAll);
            return await response.ToResult<List<GetAllWarehousesResponse>>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditWarehouseCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.WarehousesEndpoints.Save, request);
            return await response.ToResult<int>();
        }



    }
}
