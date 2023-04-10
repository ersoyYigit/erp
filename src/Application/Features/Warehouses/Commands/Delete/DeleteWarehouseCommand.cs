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

namespace ArdaManager.Application.Features.Warehouses.Commands.Delete
{
    public class DeleteWarehouseCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    internal class DeleteWarehouseCommandHandler : IRequestHandler<DeleteWarehouseCommand, Result<int>>
    {
        
        private readonly IStringLocalizer<DeleteWarehouseCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public DeleteWarehouseCommandHandler(IUnitOfWork<int> unitOfWork, IStringLocalizer<DeleteWarehouseCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(DeleteWarehouseCommand command, CancellationToken cancellationToken)
        {
            //var isWarehouseUsed = await _productRepository.IsWarehouseUsed(command.Id);
            if (true)
            {
                var warehouse = await _unitOfWork.Repository<Warehouse>().GetByIdAsync(command.Id);
                if (warehouse != null)
                {
                    await _unitOfWork.Repository<Warehouse>().DeleteAsync(warehouse);
                    await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllWarehousesCacheKey);
                    return await Result<int>.SuccessAsync(warehouse.Id, _localizer["Warehouse Deleted"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["Warehouse Not Found!"]);
                }
            }
            else
            {
                return await Result<int>.FailAsync(_localizer["Deletion Not Allowed"]);
            }
        }
    }
}
