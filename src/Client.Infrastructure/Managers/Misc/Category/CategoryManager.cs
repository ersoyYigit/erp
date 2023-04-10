using ArdaManager.Application.Features.Categories.Commands.AddEdit;
using ArdaManager.Application.Features.Categories.Queries.GetAll;
using ArdaManager.Application.Features.Categories.Queries.GetById;
using ArdaManager.Application.Features.Categories.Queries.GetCategoriesByType;
using ArdaManager.Client.Infrastructure.Extensions;
using ArdaManager.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Client.Infrastructure.Managers.Misc.Category
{

    public class CategoryManager : ICategoryManager
    {
        private readonly HttpClient _httpClient;

        public CategoryManager(HttpClient httpClient)
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
            var response = await _httpClient.DeleteAsync($"{Routes.CategoriesEndpoints.Delete}/{id}");
            return await response.ToResult<int>();
        }

        public async Task<IResult<List<GetAllCategoriesResponse>>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync(Routes.CategoriesEndpoints.GetAll);
            return await response.ToResult<List<GetAllCategoriesResponse>>();
        }

        public async Task<IResult<GetCategoryByIdResponse>> GetByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"{Routes.CategoriesEndpoints.GetById}/{id}");
            return await response.ToResult<GetCategoryByIdResponse> ();
        }

        public async Task<IResult<List<GetCategoriesByTypeResponse>>> GetByTypeAsync(GetCategoriesByTypeQuery query)
        {
            var response = await _httpClient.GetAsync(Routes.CategoriesEndpoints.GetByType(query.Type));
            return await response.ToResult<List<GetCategoriesByTypeResponse>>();
        }


        public async Task<IResult<int>> SaveAsync(AddEditCategoryCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.CategoriesEndpoints.Save, request);
            return await response.ToResult<int>();
        }


        /*
public Task<IResult<int>> ImportAsync(ImportProductCategoriesCommand request)
{
   throw new NotImplementedException();
}
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
