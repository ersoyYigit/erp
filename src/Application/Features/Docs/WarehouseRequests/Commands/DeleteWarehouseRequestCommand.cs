using ArdaManager.Application.Interfaces.Repositories;
using ArdaManager.Domain.Entities.Transactions.WarehouseDocs;
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

namespace ArdaManager.Application.Features.Docs.WarehouseRequests.Commands
{
    
    public class DeleteWarehouseRequestCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    internal class DeleteWarehouseRequestCommandHandler : IRequestHandler<DeleteWarehouseRequestCommand, Result<int>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IStringLocalizer<DeleteWarehouseRequestCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public DeleteWarehouseRequestCommandHandler(IUnitOfWork<int> unitOfWork, IProductRepository productRepository, IStringLocalizer<DeleteWarehouseRequestCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _productRepository = productRepository;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(DeleteWarehouseRequestCommand command, CancellationToken cancellationToken)
        {

            var warehouseRequest = await _unitOfWork.Repository<WarehouseRequest>().GetByIdAsync(command.Id);
            if (warehouseRequest != null)
            {
                await _unitOfWork.Repository<WarehouseRequest>().DeleteAsync(warehouseRequest);
                await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllWarehouseRequestsCacheKey);
                return await Result<int>.SuccessAsync(warehouseRequest.Id, _localizer["Depo Talep Fişi silindi"]);
            }
            else
            {
                return await Result<int>.FailAsync(_localizer["Depo talep fişi bulunamadı!"]);
            }

        }
    }
}
