using System.Threading.Tasks;

namespace ArdaManager.Application.Interfaces.Repositories
{
    public interface IProductRepository
    {
        Task<bool> IsPatternUsed(int productCategoryId);
        Task<bool> IsProductCategoryUsed(int productCategoryId);
        Task<bool> IsCategoryUsed(int categoryId);
        Task<bool> IsTemplate(int patternId);
        Task<bool> HasTemplate(int productCategoryId);



    }
}