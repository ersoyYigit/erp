using ArdaManager.Application.Features.Persons.Commands.AddEdit;
using ArdaManager.Application.Features.Persons.Queries.GetAllPaged;
using ArdaManager.Application.Requests.Human;
using ArdaManager.Client.Infrastructure.Extensions;
using ArdaManager.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Client.Infrastructure.Managers.Human.Person
{
    public  class PersonManager : IPersonManager
    {
        private readonly HttpClient _httpClient;

        public PersonManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{Routes.PersonsEndpoints.Delete}/{id}");
            return await response.ToResult<int>();
        }

        public async Task<IResult<string>> ExportToExcelAsync(string searchString = "")
        {
            var response = await _httpClient.GetAsync(string.IsNullOrWhiteSpace(searchString)
                ? Routes.PersonsEndpoints.Export
                : Routes.PersonsEndpoints.ExportFiltered(searchString));
            return await response.ToResult<string>();
        }

        public async Task<IResult<string>> GetPersonImageAsync(int id)
        {
            var response = await _httpClient.GetAsync(Routes.PersonsEndpoints.GetPersonImage(id));
            return await response.ToResult<string>();
        }

        public async Task<PaginatedResult<GetAllPagedPersonsResponse>> GetPersonsAsync(GetAllPagedPersonsRequest request)
        {
            var response = await _httpClient.GetAsync(Routes.PersonsEndpoints.GetAllPaged(request.PageNumber, request.PageSize, request.SearchString, request.Orderby));
            return await response.ToPaginatedResult<GetAllPagedPersonsResponse>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditPersonCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.PersonsEndpoints.Save, request);
            return await response.ToResult<int>();
        }
    }
}
