using ArdaManager.Application.Features.Categories.Queries.GetCategoriesByType;
using ArdaManager.Application.Features.RecipeItems.Commands.AddEdit;
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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ArdaManager.Client.Infrastructure.Managers.Catalog.RecipeItem
{
    public class RecipeItemManager : IRecipeItemManager
    {
        private readonly HttpClient _httpClient;

        public RecipeItemManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }



        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{Routes.RecipeItemsEndpoints.Delete}/{id}");
            return await response.ToResult<int>();
        }

        public async Task<IResult<List<GetRecipeItemsByOwnerProductIdResponse>>> GetByOwnerProductIdAsync(GetRecipeItemsByOwnerProductIdQuery query)
        {
            var response = await _httpClient.GetAsync(Routes.RecipeItemsEndpoints.GetByOwnerProductId(query.OwnerProductId));
            return await response.ToResult<List<GetRecipeItemsByOwnerProductIdResponse>>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditRecipeItemCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.RecipeItemsEndpoints.Save, request);
            return await response.ToResult<int>();
        }
    }
}
