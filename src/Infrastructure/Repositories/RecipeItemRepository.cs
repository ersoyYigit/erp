using ArdaManager.Application.Interfaces.Repositories;
using ArdaManager.Domain.Entities.Catalog;
using ArdaManager.Domain.Entities.Misc;
using ArdaManager.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Infrastructure.Repositories
{
    internal class RecipeItemRepository : IRecipeItemRepository
    {
        private readonly IRepositoryAsync<RecipeItem, int> _repository;

        public RecipeItemRepository(IRepositoryAsync<RecipeItem, int> repository)
        {
            _repository = repository;
        }

        public async Task<List<RecipeItem>> GetRecipeItemsByOwnerProductId(int ownerProductId)
        {
            List<RecipeItem> ret = await Task.Run(() => 
                _repository
                    .Entities
                    .Include("OwnerProduct")
                    .Include("RecipeProduct")
                    .Include("Warehouse")
                    .Where(c => c.OwnerProductId == ownerProductId).ToList()
            );

            return ret;
        }

        public async Task<RecipeItem> GetRecipeItemsById(int id)
        {
            RecipeItem ret = await Task.Run(() =>
                _repository
                    .Entities
                    .Include("OwnerProduct")
                    .Include("RecipeProduct")
                    .Include("Warehouse")
                    .Where(c => c.Id == id).FirstOrDefaultAsync()
            ); ;

            return ret;
        }
    }
}
