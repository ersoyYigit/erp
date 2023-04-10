using ArdaManager.Application.Interfaces.Repositories;
using ArdaManager.Domain.Entities.Corporation;
using ArdaManager.Shared.Constants.Application;
using ArdaManager.Shared.Wrapper;
using MediatR;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ArdaManager.Application.Features.Fairs.Commands.Delete
{
    public class DeleteFairCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    internal class DeleteFairCommandHandler : IRequestHandler<DeleteFairCommand, Result<int>>
    {
        private readonly IStringLocalizer<DeleteFairCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public DeleteFairCommandHandler(IUnitOfWork<int> unitOfWork, IStringLocalizer<DeleteFairCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(DeleteFairCommand command, CancellationToken cancellationToken)
        {
            var isFairUsed = false;//await _productRepository.IsFairUsed(command.Id);
            if (!isFairUsed)
            {
                var fair = await _unitOfWork.Repository<Fair>().GetByIdAsync(command.Id);
                if (fair != null)
                {
                    await _unitOfWork.Repository<Fair>().DeleteAsync(fair);
                    await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllFairsCacheKey);
                    return await Result<int>.SuccessAsync(fair.Id, _localizer["Fair Deleted"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["Fair Not Found!"]);
                }
            }
            else
            {
                return await Result<int>.FailAsync(_localizer["Deletion Not Allowed"]);
            }
        }
    }
}
