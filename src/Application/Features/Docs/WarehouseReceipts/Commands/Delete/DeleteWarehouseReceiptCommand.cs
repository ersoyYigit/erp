using ArdaManager.Application.Features.Docs.WarehouseReceipts.Commands.Delete;
using ArdaManager.Application.Interfaces.Repositories;
using ArdaManager.Domain.Entities.Transactions.WarehouseDocs;
using ArdaManager.Shared.Constants.Application;
using ArdaManager.Shared.Wrapper;
using Azure.Core;
using MediatR;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ArdaManager.Application.Features.Docs.WarehouseReceipts.Commands.Delete
{
    public class DeleteWarehouseReceiptCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    internal class DeleteWarehouseReceiptCommandHandler : IRequestHandler<DeleteWarehouseReceiptCommand, Result<int>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IStringLocalizer<DeleteWarehouseReceiptCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public DeleteWarehouseReceiptCommandHandler(IUnitOfWork<int> unitOfWork, IProductRepository productRepository, IStringLocalizer<DeleteWarehouseReceiptCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _productRepository = productRepository;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(DeleteWarehouseReceiptCommand command, CancellationToken cancellationToken)
        {

            var warehouseReceipt = await _unitOfWork.Repository<WarehouseReceipt>().GetByIdAsync(command.Id);
            if (warehouseReceipt != null)
            {
                await _unitOfWork.Repository<WarehouseReceipt>().DeleteAsync(warehouseReceipt);
                await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllWarehouseReceiptsCacheKey + warehouseReceipt.WarehouseReceiptType.ToString());
                return await Result<int>.SuccessAsync(warehouseReceipt.Id, _localizer["Depo Fişi silindi"]);
            }
            else
            {
                return await Result<int>.FailAsync(_localizer["Depo fişi  bulunamadı!"]);
            }

        }
    }
}
