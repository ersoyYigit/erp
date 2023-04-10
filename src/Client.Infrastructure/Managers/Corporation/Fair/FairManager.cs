using ArdaManager.Application.Features.Fairs.Commands.AddEdit;
using ArdaManager.Application.Features.Fairs.Queries.GetAll;
using ArdaManager.Client.Infrastructure.Extensions;
using ArdaManager.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Client.Infrastructure.Managers.Corporation.Fair
{
    public class FairManager : IFairManager
    {
        private readonly HttpClient _httpClient;

        public FairManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{Routes.FairsEndpoints.Delete}/{id}");
            return await response.ToResult<int>();
        }

        public async Task<IResult<List<GetAllFairsResponse>>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync(Routes.FairsEndpoints.GetAll);
            return await response.ToResult<List<GetAllFairsResponse>>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditFairCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.FairsEndpoints.Save, request);
            return await response.ToResult<int>();
        }


    }
}
