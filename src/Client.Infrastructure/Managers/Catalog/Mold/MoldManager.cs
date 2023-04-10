using ArdaManager.Application.Features.Products.Queries.GetAllPaged;
using ArdaManager.Application.Features.Molds.Commands.AddEdit;
using ArdaManager.Application.Features.Molds.Queries.GetAllPaged;
using ArdaManager.Application.Requests.Catalog;
using ArdaManager.Client.Infrastructure.Extensions;
using ArdaManager.Shared.Wrapper;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ArdaManager.Client.Infrastructure.Managers.Catalog.Mold
{
    public class MoldManager : IMoldManager
    {
        private readonly HttpClient _httpClient;

        public MoldManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{Routes.MoldsEndpoints.Delete}/{id}");
            return await response.ToResult<int>();
        }

        public async Task<IResult<string>> ExportToExcelAsync(string searchString = "")
        {
            var response = await _httpClient.GetAsync(string.IsNullOrWhiteSpace(searchString)
                ? Routes.MoldsEndpoints.Export
                : Routes.MoldsEndpoints.ExportFiltered(searchString));
            return await response.ToResult<string>();
        }

        public async Task<IResult<string>> GetMoldImageAsync(int id)
        {
            var response = await _httpClient.GetAsync(Routes.MoldsEndpoints.GetMoldImage(id));
            return await response.ToResult<string>();
        }

        public async Task<PaginatedResult<GetAllPagedMoldsResponse>> GetMoldsAsync(GetAllMoldsRequest request)
        {
            var response = await _httpClient.GetAsync(Routes.MoldsEndpoints.GetAllPaged(request.PageNumber, request.PageSize, request.SearchString, request.Orderby));
            return await response.ToPaginatedResult<GetAllPagedMoldsResponse>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditMoldCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.MoldsEndpoints.Save, request);
            return await response.ToResult<int>();
        }

    }
}