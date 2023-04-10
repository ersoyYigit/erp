using ArdaManager.Application.Features.Patterns.Commands.AddEdit;
using ArdaManager.Application.Features.Patterns.Queries.GetAll;
using ArdaManager.Application.Features.Taxes.Commands;
using ArdaManager.Application.Features.Taxes.Queries;
using ArdaManager.Client.Infrastructure.Extensions;
using ArdaManager.Client.Infrastructure.Managers.Catalog;
using ArdaManager.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Client.Infrastructure.Managers.Misc.Tax
{
    public class TaxManager : ITaxManager
    {
        private readonly HttpClient _httpClient;

        public TaxManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        
        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{Routes.TaxesEndpoints.Delete}/{id}");
            return await response.ToResult<int>();
        }

        public async Task<IResult<List<GetAllTaxesResponse>>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync(Routes.TaxesEndpoints.GetAll);
            return await response.ToResult<List<GetAllTaxesResponse>>();
        }

        public async Task<IResult<int>> SaveAsync(UpsertTaxCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.TaxesEndpoints.Save, request);
            return await response.ToResult<int>();
        }

    }
}
