using ArdaManager.Application.Features.Companies.Commands.AddEdit;
using ArdaManager.Application.Features.Companies.Queries.GetAllPaged;
using ArdaManager.Application.Features.Companies.Queries.Search;
using ArdaManager.Application.Features.Products.Queries.Search;
using ArdaManager.Application.Requests.Corporation;
using ArdaManager.Client.Infrastructure.Extensions;
using ArdaManager.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Client.Infrastructure.Managers.Corporation.Company
{
    public class CompanyManager : ICompanyManager
    {
        private readonly HttpClient _httpClient;

        public CompanyManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{Routes.CompaniesEndpoints.Delete}/{id}");
            return await response.ToResult<int>();
        }

        public async Task<IResult<string>> ExportToExcelAsync(string searchString = "")
        {
            var response = await _httpClient.GetAsync(string.IsNullOrWhiteSpace(searchString)
                ? Routes.CompaniesEndpoints.Export
                : Routes.CompaniesEndpoints.ExportFiltered(searchString));
            return await response.ToResult<string>();
        }

        /*
        public async Task<IResult<string>> GetCompanyImageAsync(int id)
        {
            var response = await _httpClient.GetAsync(Routes.CompaniesEndpoints.GetCompanyImage(id));
            return await response.ToResult<string>();
        }*/

        public async Task<PaginatedResult<GetAllPagedCompaniesResponse>> GetCompaniesAsync(GetAllPagedCompaniesRequest request)
        {
            var response = await _httpClient.GetAsync(Routes.CompaniesEndpoints.GetAllPaged(request.PageNumber, request.PageSize, request.SearchString, request.Orderby));
            return await response.ToPaginatedResult<GetAllPagedCompaniesResponse>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditCompanyCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.CompaniesEndpoints.Save, request);
            return await response.ToResult<int>();
        }

        public async Task<IResult<List<CompanySearchResultDto>>> GetFilteredAsync(CompanySearchQuery query)
        {
            // POST 
            //var response = await _httpClient.PostAsJsonAsync(Routes.CompaniesEndpoints.GetFilteredPost, query);

            var response = await _httpClient.PostAsJsonAsync(Routes.CompaniesEndpoints.GetFilteredPost, query);
            //var response = await _httpClient.GetAsync(Routes.CompaniesEndpoints.GetFiltered(query));

            return await response.ToResult<List<CompanySearchResultDto>>();
        }
    }
}
