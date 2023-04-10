using ArdaManager.Application.Interfaces.Repositories;
using ArdaManager.Domain.Entities.Inventory;
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

namespace ArdaManager.Application.Features.Racks.Commands.Delete
{
    public class DeleteRackCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    internal class DeleteRackCommandHandler : IRequestHandler<DeleteRackCommand, Result<int>>
    {
        private readonly IStringLocalizer<DeleteRackCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public DeleteRackCommandHandler(IUnitOfWork<int> unitOfWork, IStringLocalizer<DeleteRackCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(DeleteRackCommand command, CancellationToken cancellationToken)
        {
            var isRackUsed = false;//await _productRepository.IsRackUsed(command.Id);
            if (!isRackUsed)
            {
                var rack = await _unitOfWork.Repository<Rack>().GetByIdAsync(command.Id);
                if (rack != null)
                {
                    await _unitOfWork.Repository<Rack>().DeleteAsync(rack);
                    await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllRacksCacheKey);
                    return await Result<int>.SuccessAsync(rack.Id, _localizer["Rack Deleted"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["Rack Not Found!"]);
                }
            }
            else
            {
                return await Result<int>.FailAsync(_localizer["Deletion Not Allowed"]);
            }
        }
    }
}
