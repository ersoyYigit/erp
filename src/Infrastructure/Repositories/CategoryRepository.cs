using ArdaManager.Application.Interfaces.Repositories;
using ArdaManager.Domain.Entities.Misc;
using ArdaManager.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Infrastructure.Repositories
{
    internal class CategoryRepository : ICategoryRepository
    {
        private readonly IRepositoryAsync<Category, int> _repository;

        public CategoryRepository(IRepositoryAsync<Category, int> repository)
        {
            _repository = repository;
        }

        public async Task<List<Category>> GetCategoriesByType(int Type)
        {
            List<Category> ret = await Task.Run(() => _repository.Entities.Where(c => c.Type == (CategoryType)Type).ToList());
            ret = ret.Where(x => x.SubCategories == null).ToList();
            return ret;
        }
    }
}
