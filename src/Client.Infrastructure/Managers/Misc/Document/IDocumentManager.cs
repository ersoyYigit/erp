using ArdaManager.Application.Features.Documents.Commands.AddEdit;
using ArdaManager.Application.Features.Documents.Queries.GetAll;
using ArdaManager.Application.Requests.Documents;
using ArdaManager.Shared.Wrapper;
using System.Threading.Tasks;
using ArdaManager.Application.Features.Documents.Queries.GetById;

namespace ArdaManager.Client.Infrastructure.Managers.Misc.Document
{
    public interface IDocumentManager : IManager
    {
        Task<PaginatedResult<GetAllDocumentsResponse>> GetAllAsync(GetAllPagedDocumentsRequest request);

        Task<IResult<GetDocumentByIdResponse>> GetByIdAsync(GetDocumentByIdQuery request);

        Task<IResult<int>> SaveAsync(AddEditDocumentCommand request);

        Task<IResult<int>> DeleteAsync(int id);
    }
}