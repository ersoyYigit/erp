using ArdaManager.Domain.Entities.Catalog;
using ArdaManager.Domain.Entities.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Application.Interfaces.Repositories
{
    public interface IRecipeItemRepository
    {
        Task<List<RecipeItem>> GetRecipeItemsByOwnerProductId(int ownerProductId);
        Task<RecipeItem> GetRecipeItemsById(int id);
    }
}
