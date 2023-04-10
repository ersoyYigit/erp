using ArdaManager.Application.Features.Products.Commands.AddEdit;
using ArdaManager.Application.Features.Products.Queries.GetAllPaged;
using ArdaManager.Application.Requests.Catalog;
using ArdaManager.Client.Infrastructure.Extensions;
using ArdaManager.Shared.Wrapper;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using ArdaManager.Application.Features.Patterns.Queries.GetAll;
using ArdaManager.Application.Features.Patterns.Commands.AddEdit;

namespace ArdaManager.Client.Infrastructure.Managers.Catalog
{
    public interface IPatternManager : IManager
    {
        Task<IResult<List<GetAllPatternsResponse>>> GetAllAsync();

        Task<IResult<int>> SaveAsync(AddEditPatternCommand request);

        Task<IResult<int>> DeleteAsync(int id);

        
        Task<IResult<string>> ExportToExcelAsync(string searchString = "");
        
        Task<IResult<int>> ImportAsync(ImportPatternsCommand request);
    }
}