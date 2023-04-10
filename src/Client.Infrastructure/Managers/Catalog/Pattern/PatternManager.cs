using ArdaManager.Application.Features.Patterns.Queries.GetAll;
using ArdaManager.Client.Infrastructure.Extensions;
using ArdaManager.Shared.Wrapper;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using ArdaManager.Application.Features.Patterns.Commands.AddEdit;
//using ArdaManager.Application.Features.Patterns.Commands.Import;

namespace ArdaManager.Client.Infrastructure.Managers.Catalog.Pattern
{
    public class PatternManager : IPatternManager
    {
        private readonly HttpClient _httpClient;

        public PatternManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<string>> ExportToExcelAsync(string searchString = "")
        {
            var response = await _httpClient.GetAsync(string.IsNullOrWhiteSpace(searchString)
                ? Routes.PatternsEndpoints.Export
                : Routes.PatternsEndpoints.ExportFiltered(searchString));
            return await response.ToResult<string>();
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{Routes.PatternsEndpoints.Delete}/{id}");
            return await response.ToResult<int>();
        }

        public async Task<IResult<List<GetAllPatternsResponse>>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync(Routes.PatternsEndpoints.GetAll);
            return await response.ToResult<List<GetAllPatternsResponse>>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditPatternCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.PatternsEndpoints.Save, request);
            return await response.ToResult<int>();
        }

        public Task<IResult<int>> ImportAsync(ImportPatternsCommand request)
        {
            throw new System.NotImplementedException();
        }

        /*
        public async Task<IResult<int>> ImportAsync(ImportPatternsCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.PatternsEndpoints.Import, request);
            return await response.ToResult<int>();
        }*/
    }
}