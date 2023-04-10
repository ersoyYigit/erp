using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArdaManager.Application.Features.ProductCategories.Commands.AddEdit;
using ArdaManager.Application.Features.ProductCategories.Queries.GetAll;
using ArdaManager.Application.Features.Products.Commands.AddEdit;
using ArdaManager.Shared.Wrapper;

namespace ArdaManager.Client.Infrastructure.Managers.Catalog.ProductCategory
{
    
    public interface IProductCategoryManager : IManager
    {
        Task<IResult<List<GetAllProductCategoriesResponse>>> GetAllAsync();

        Task<IResult<int>> SaveAsync(AddEditProductCategoryCommand request);

        Task<IResult<int>> DeleteAsync(int id);

        /*
        Task<IResult<string>> ExportToExcelAsync(string searchString = "");
        */
        Task<IResult<int>> ImportAsync(ImportProductCategoriesCommand request);
    }
}
