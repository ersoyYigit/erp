using ArdaManager.Application.Extensions;
using ArdaManager.Application.Interfaces.Repositories;
using ArdaManager.Domain.Entities.Catalog;
using ArdaManager.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ArdaManager.Application.Features.Templates.Queries.GetTemplateImage
{
    public class GetTemplateImageQuery : IRequest<Result<string>>
    {
        public int Id { get; set; }

        public GetTemplateImageQuery(int productId)
        {
            Id = productId;
        }
    }

    internal class GetTemplateImageQueryHandler : IRequestHandler<GetTemplateImageQuery, Result<string>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;

        public GetTemplateImageQueryHandler(IUnitOfWork<int> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<string>> Handle(GetTemplateImageQuery request, CancellationToken cancellationToken)
        {
            var data = await _unitOfWork.Repository<Template>().Entities.Where(p => p.Id == request.Id).Select(a => a.ImageDataURL).FirstOrDefaultAsync(cancellationToken);
            data = data.ToRelativePath();
            return await Result<string>.SuccessAsync(data: data);
        }
    }
}