
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using ArdaManager.Application.Features.ProductCategories.Commands.AddEdit;
using ArdaManager.Application.Features.ProductCategories.Queries.GetAll;
using ArdaManager.Application.Features.Products.Commands.AddEdit;
using ArdaManager.Client.Infrastructure.Extensions;
using ArdaManager.Shared.Wrapper;

namespace ArdaManager.Client.Infrastructure.Managers.Catalog.ProductCategory
{
    
    public class ProductCategoryManager : IProductCategoryManager
    {
        private readonly HttpClient _httpClient;

        public ProductCategoryManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /*        public async Task<IResult<string>> ExportToExcelAsync(string searchString = "")
                {
                    var response = await _httpClient.GetAsync(string.IsNullOrWhiteSpace(searchString)
                        ? Routes.ProductCategoriesEndpoints.Export
                        : Routes.ProductCategoriesEndpoints.ExportFiltered(searchString));
                    return await response.ToResult<string>();
                }
        */
        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{Routes.ProductCategoriesEndpoints.Delete}/{id}");
            return await response.ToResult<int>();
        }

        public async Task<IResult<List<GetAllProductCategoriesResponse>>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync(Routes.ProductCategoriesEndpoints.GetAll);
            return await response.ToResult<List<GetAllProductCategoriesResponse>>();
        }


        public async Task<IResult<int>> SaveAsync(AddEditProductCategoryCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.ProductCategoriesEndpoints.Save, request);
            return await response.ToResult<int>();
        }
        public Task<IResult<int>> ImportAsync(ImportProductCategoriesCommand request)
        {
            throw new NotImplementedException();
        }
        /*
        Task<IResult<int>> IProductCategoryManager.ImportAsync(ImportProductCategoriesCommand request)
        {
            throw new NotImplementedException();
        }*/


        /*
        public async Task<IResult<int>> ImportAsync(ImportProductCategoriesCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.ProductCategoriesEndpoints.Import, request);
            return await response.ToResult<int>();
        }*/
    }
}
