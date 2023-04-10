using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Client.Infrastructure.Routes
{
    public static class RecipeItemsEndpoints
    {
        public static string GetAll = BaseEndpoint.Server + "api/v1/RecipeItems";
        public static string Delete = BaseEndpoint.Server + "api/v1/RecipeItems";
        public static string Save = BaseEndpoint.Server + "api/v1/RecipeItems";
        public static string GetById = BaseEndpoint.Server + "api/v1/RecipeItems";
        public static string GetByOwnerProductId(int id)
        {
            return BaseEndpoint.Server + $"api/v1/RecipeItems/GetByOwnerProductId/{id}";
        }
    }
}
