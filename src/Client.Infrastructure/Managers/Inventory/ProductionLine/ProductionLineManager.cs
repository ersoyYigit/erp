using ArdaManager.Application.Features.ProductionLines.Commands.AddEdit;
using ArdaManager.Application.Features.ProductionLines.Queries.GetAll;
using ArdaManager.Application.Features.ProductionLines.Queries.GetById;
using ArdaManager.Application.Features.RecipeItems.Queries.GetByOwnerProductId;
using ArdaManager.Client.Infrastructure.Extensions;
using ArdaManager.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Client.Infrastructure.Managers.Inventory.ProductionLine
{
    public class ProductionLineManager : IProductionLineManager
    {
        private readonly HttpClient _httpClient;

        public ProductionLineManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }



        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{Routes.ProductionLinesEndpoints.Delete}/{id}");
            return await response.ToResult<int>();
        }

        public Task<IResult<string>> ExportToExcelAsync(string searchString = "")
        {
            throw new NotImplementedException();
        }

        public async Task<IResult<List<GetAllProductionLinesResponse>>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync(Routes.ProductionLinesEndpoints.GetAll);
            return await response.ToResult<List<GetAllProductionLinesResponse>>();
        }

        public async Task<IResult<GetAllProductionLinesResponse>> GetByIdAsync()
        {
            var response = await _httpClient.GetAsync(Routes.ProductionLinesEndpoints.GetAll);
            return await response.ToResult<GetAllProductionLinesResponse>();
        }

        public async Task<IResult<GetProductionLineByIdResponse>> GetByIdAsync(GetProductionLineByIdQuery query)
        {
            var response = await _httpClient.GetAsync(Routes.ProductionLinesEndpoints.GetById(query.Id));
            return await response.ToResult<GetProductionLineByIdResponse>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditProductionLineCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.ProductionLinesEndpoints.Save, request);
            return await response.ToResult<int>();
        }

    }
}
