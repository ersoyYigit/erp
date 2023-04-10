using ArdaManager.Application.Interfaces.Repositories;
using ArdaManager.Domain.Entities.Catalog;
using ArdaManager.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;

namespace ArdaManager.Application.Features.Templates.Commands.Delete
{
    public class DeleteTemplateCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    internal class DeleteTemplateCommandHandler : IRequestHandler<DeleteTemplateCommand, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<DeleteTemplateCommandHandler> _localizer;

        public DeleteTemplateCommandHandler(IUnitOfWork<int> unitOfWork, IStringLocalizer<DeleteTemplateCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(DeleteTemplateCommand command, CancellationToken cancellationToken)
        {
            var product = await _unitOfWork.Repository<Product>().GetByIdAsync(command.Id);
            if (product != null)
            {
                await _unitOfWork.Repository<Product>().DeleteAsync(product);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(product.Id, _localizer["Template Deleted"]);
            }
            else
            {
                return await Result<int>.FailAsync(_localizer["Template Not Found!"]);
            }
        }
    }
}