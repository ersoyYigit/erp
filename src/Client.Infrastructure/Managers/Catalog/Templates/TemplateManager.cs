using ArdaManager.Application.Features.Products.Queries.GetAllPaged;
using ArdaManager.Application.Features.Templates.Commands.AddEdit;
using ArdaManager.Application.Features.Templates.Queries.GetAllPaged;
using ArdaManager.Application.Requests.Catalog;
using ArdaManager.Client.Infrastructure.Extensions;
using ArdaManager.Shared.Wrapper;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ArdaManager.Client.Infrastructure.Managers.Catalog.Template
{
    public class TemplateManager : ITemplateManager
    {
        private readonly HttpClient _httpClient;

        public TemplateManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{Routes.TemplatesEndpoints.Delete}/{id}");
            return await response.ToResult<int>();
        }

        public async Task<IResult<string>> ExportToExcelAsync(string searchString = "")
        {
            var response = await _httpClient.GetAsync(string.IsNullOrWhiteSpace(searchString)
                ? Routes.TemplatesEndpoints.Export
                : Routes.TemplatesEndpoints.ExportFiltered(searchString));
            return await response.ToResult<string>();
        }

        public async Task<IResult<string>> GetTemplateImageAsync(int id)
        {
            var response = await _httpClient.GetAsync(Routes.TemplatesEndpoints.GetTemplateImage(id));
            return await response.ToResult<string>();
        }

        public async Task<PaginatedResult<GetAllPagedTemplatesResponse>> GetTemplatesAsync(GetAllPagedTemplatesRequest request)
        {
            var response = await _httpClient.GetAsync(Routes.TemplatesEndpoints.GetAllPaged(request.PageNumber, request.PageSize, request.SearchString, request.Orderby));
            return await response.ToPaginatedResult<GetAllPagedTemplatesResponse>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditTemplateCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.TemplatesEndpoints.Save, request);
            return await response.ToResult<int>();
        }

    }
}