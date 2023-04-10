using ArdaManager.Application.Interfaces.Repositories;
using ArdaManager.Domain.Entities.Catalog;
using ArdaManager.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using ArdaManager.Shared.Constants.Application;

namespace ArdaManager.Application.Features.Patterns.Commands.Delete
{
    public class DeletePatternCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    internal class DeletePatternCommandHandler : IRequestHandler<DeletePatternCommand, Result<int>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IStringLocalizer<DeletePatternCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public DeletePatternCommandHandler(IUnitOfWork<int> unitOfWork, IProductRepository productRepository, IStringLocalizer<DeletePatternCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _productRepository = productRepository;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(DeletePatternCommand command, CancellationToken cancellationToken)
        {
            var isPatternUsed = await _productRepository.IsPatternUsed(command.Id);
            if (!isPatternUsed)
            {
                var pattern = await _unitOfWork.Repository<Pattern>().GetByIdAsync(command.Id);
                if (pattern != null)
                {
                    await _unitOfWork.Repository<Pattern>().DeleteAsync(pattern);
                    await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllPatternsCacheKey);
                    return await Result<int>.SuccessAsync(pattern.Id, _localizer["Pattern Deleted"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["Pattern Not Found!"]);
                }
            }
            else
            {
                return await Result<int>.FailAsync(_localizer["Deletion Not Allowed"]);
            }
        }
    }
}