using ArdaManager.Application.Features.Products.Commands.AddEdit;
using ArdaManager.Application.Features.Products.Queries.GetAllPaged;
using ArdaManager.Application.Features.Molds.Commands.AddEdit;
using ArdaManager.Application.Features.Molds.Queries.GetAllPaged;
using ArdaManager.Application.Requests.Catalog;
using ArdaManager.Shared.Wrapper;
using System.Threading.Tasks;

namespace ArdaManager.Client.Infrastructure.Managers.Catalog.Mold
{
    public interface IMoldManager : IManager
    {
        Task<PaginatedResult<GetAllPagedMoldsResponse>> GetMoldsAsync(GetAllMoldsRequest request);

        Task<IResult<string>> GetMoldImageAsync(int id);

        Task<IResult<int>> SaveAsync(AddEditMoldCommand request);

        Task<IResult<int>> DeleteAsync(int id);

    }
}