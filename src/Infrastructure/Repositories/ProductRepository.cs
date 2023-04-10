using ArdaManager.Application.Interfaces.Repositories;
using ArdaManager.Domain.Entities.Catalog;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ArdaManager.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IRepositoryAsync<Product, int> _repository;

        public ProductRepository(IRepositoryAsync<Product, int> repository)
        {
            _repository = repository;
        }

        public async Task<bool> IsPatternUsed(int patternId)
        {
            return await _repository.Entities.AnyAsync(b => b.PatternId == patternId);
        }
        public async Task<bool> HasTemplate(int patternId)
        {
            return await _repository.Entities.AnyAsync(b => b.TemplateId != null);
        }

        public async Task<bool> IsTemplate(int patternId)
        {
            return await _repository.Entities.AnyAsync(b => b.Type == Domain.Enums.ProductType.Template);
        }

        public async Task<bool> IsProductCategoryUsed(int productCategoryId)
        {
            return false;
            //return await _repository.Entities.AnyAsync(b => b.ProductCategoryId == productCategoryId);
        }
        public async Task<bool> IsCategoryUsed(int categoryId)
        {
            
            return await _repository.Entities.AnyAsync(b => b.CategoryId == categoryId);
        }
    }
}