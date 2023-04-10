using ArdaManager.Application.Features.Products.Commands.AddEdit;
using ArdaManager.Application.Features.Products.Queries.GetAllPaged;
using ArdaManager.Application.Features.Products.Queries.Search;
using ArdaManager.Application.Requests.Catalog;
using ArdaManager.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ArdaManager.Client.Infrastructure.Managers.Catalog.Product
{

    /// <summary>
    /// TODO : fix product
    /// </summary>
    public interface IProductManager : IManager
    {
        Task<PaginatedResult<GetAllPagedProductsResponse>> GetProductsAsync(GetAllPagedProductsRequest request);

        Task<IResult<string>> GetProductImageAsync(int id);

        Task<IResult<int>> SaveAsync(AddEditProductCommand request);

        Task<IResult<int>> DeleteAsync(int id);

        Task<IResult<string>> ExportToExcelAsync(string searchString = "");
        Task<IResult<List<ProductSearchResultDto>>> GetFilteredAsync(string searchTerm);
    }
}