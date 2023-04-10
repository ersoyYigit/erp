using ArdaManager.Application.Features.Products.Commands.AddEdit;
using ArdaManager.Application.Features.Products.Queries.GetAllPaged;
using ArdaManager.Application.Features.Templates.Commands.AddEdit;
using ArdaManager.Application.Features.Templates.Queries.GetAllPaged;
using ArdaManager.Application.Requests.Catalog;
using ArdaManager.Shared.Wrapper;
using System.Threading.Tasks;

namespace ArdaManager.Client.Infrastructure.Managers.Catalog.Template
{
    public interface ITemplateManager : IManager
    {
        Task<PaginatedResult<GetAllPagedTemplatesResponse>> GetTemplatesAsync(GetAllPagedTemplatesRequest request);

        Task<IResult<string>> GetTemplateImageAsync(int id);

        Task<IResult<int>> SaveAsync(AddEditTemplateCommand request);

        Task<IResult<int>> DeleteAsync(int id);

    }
}