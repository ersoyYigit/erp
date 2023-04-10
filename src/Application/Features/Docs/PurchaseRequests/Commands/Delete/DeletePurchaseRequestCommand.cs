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

namespace ArdaManager.Application.Features.Docs.PurchaseRequests.Commands.Delete
{
    public class DeletePurchaseRequestCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }


    public class DeletePurchaseRequestCommandHandler : IRequestHandler<DeletePurchaseRequestCommand, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;

        public DeletePurchaseRequestCommandHandler(IUnitOfWork<int> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<int>> Handle(DeletePurchaseRequestCommand command, CancellationToken cancellationToken)
        {
            var existingPurchaseRequest = await _unitOfWork.Repository<PurchaseRequest>().GetByIdAsync(command.Id);

            if (existingPurchaseRequest == null)
            {
                return await Result<int>.FailAsync("Satınalma talebi bulunamadı!");
            }

            // Delete related PurchaseRequestRow records
            var relatedRows = await _unitOfWork.Repository<PurchaseRequestRow>().GetAllAsync(x => x.PurchaseRequestId == command.Id);
            await _unitOfWork.Repository<PurchaseRequestRow>().DeleteRangeAsync(relatedRows);

            await _unitOfWork.Repository<PurchaseRequest>().DeleteAsync(existingPurchaseRequest);
            //await _unitOfWork.Commit(cancellationToken);
            await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllPurchaseRequestsCacheKey);

            return await Result<int>.SuccessAsync(existingPurchaseRequest.Id, "Talep Silindi!");
        }
    }
}
