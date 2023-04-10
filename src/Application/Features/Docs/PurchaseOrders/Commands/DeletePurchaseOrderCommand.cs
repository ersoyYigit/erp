using ArdaManager.Application.Interfaces.Repositories;
using ArdaManager.Domain.Entities.Transactions.Purchase;
using ArdaManager.Shared.Constants.Application;
using ArdaManager.Shared.Wrapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ArdaManager.Application.Features.Docs.PurchaseOrders.Commands
{
    public class DeletePurchaseOrderCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    public class DeletePurchaseOrderCommandHandler : IRequestHandler<DeletePurchaseOrderCommand, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;

        public DeletePurchaseOrderCommandHandler(IUnitOfWork<int> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<int>> Handle(DeletePurchaseOrderCommand command, CancellationToken cancellationToken)
        {
            var existingPurchaseOrder = await _unitOfWork.Repository<PurchaseOrder>().GetByIdAsync(command.Id);

            if (existingPurchaseOrder == null)
            {
                return await Result<int>.FailAsync("Satınalma siparişi bulunamadı!");
            }

            // Delete related PurchaseOrderRow records
            var relatedRows = await _unitOfWork.Repository<PurchaseOrderRow>().GetAllAsync(x => x.PurchaseOrderId == command.Id);
            await _unitOfWork.Repository<PurchaseOrderRow>().DeleteRangeAsync(relatedRows);

            await _unitOfWork.Repository<PurchaseOrder>().DeleteAsync(existingPurchaseOrder);
            await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllPurchaseOrdersCacheKey);

            return await Result<int>.SuccessAsync(existingPurchaseOrder.Id, "Sipariş Silindi!");
        }
    }

}
