using ArdaManager.Application.Features.Docs.TemplateWorkOrders.Commands.AddEdit;
using ArdaManager.Application.Features.Docs.TemplateWorkOrders.Queries.GetAll;
using ArdaManager.Application.Features.Docs.TemplateWorkOrders.Queries.GetById;
using ArdaManager.Application.Features.RecipeItems.Queries.GetByOwnerProductId;
using ArdaManager.Client.Infrastructure.Extensions;
using ArdaManager.Client.Infrastructure.Routes;
using ArdaManager.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Client.Infrastructure.Managers.Docs.TemplateWorkOrder
{
    public class TemplateWorkOrderManager : ITemplateWorkOrderManager
    {
        private readonly HttpClient _httpClient;

        public TemplateWorkOrderManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }



        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{TemplateWorkOrdersEndpoints.Delete}/{id}");
            return await response.ToResult<int>();
        }

        public Task<IResult<string>> ExportToExcelAsync(string searchString = "")
        {
            throw new NotImplementedException();
        }

        public async Task<IResult<List<GetAllTemplateWorkOrdersResponse>>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync(TemplateWorkOrdersEndpoints.GetAll);
            return await response.ToResult<List<GetAllTemplateWorkOrdersResponse>>();
        }

        public async Task<IResult<GetAllTemplateWorkOrdersResponse>> GetByIdAsync()
        {
            var response = await _httpClient.GetAsync(TemplateWorkOrdersEndpoints.GetAll);
            return await response.ToResult<GetAllTemplateWorkOrdersResponse>();
        }

        public async Task<IResult<GetTemplateWorkOrderByIdResponse>> GetByIdAsync(GetTemplateWorkOrderByIdQuery query)
        {
            var response = await _httpClient.GetAsync(TemplateWorkOrdersEndpoints.GetById(query.Id));
            return await response.ToResult<GetTemplateWorkOrderByIdResponse>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditTemplateWorkOrderCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(TemplateWorkOrdersEndpoints.Save, request);
            return await response.ToResult<int>();
        }

    }
}
