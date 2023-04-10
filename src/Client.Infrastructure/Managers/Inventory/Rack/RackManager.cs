using ArdaManager.Application.Features.Racks.Commands.AddEdit;
using ArdaManager.Application.Features.Racks.Queries.GetAll;
using ArdaManager.Client.Infrastructure.Extensions;
using ArdaManager.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Client.Infrastructure.Managers.Inventory.Rack
{
    public class RackManager : IRackManager
    {
        private readonly HttpClient _httpClient;

        public RackManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }



        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{Routes.RacksEndpoints.Delete}/{id}");
            return await response.ToResult<int>();
        }

        public async Task<IResult<List<GetAllRacksResponse>>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync(Routes.RacksEndpoints.GetAll);
            return await response.ToResult<List<GetAllRacksResponse>>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditRackCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.RacksEndpoints.Save, request);
            return await response.ToResult<int>();
        }



    }
}
