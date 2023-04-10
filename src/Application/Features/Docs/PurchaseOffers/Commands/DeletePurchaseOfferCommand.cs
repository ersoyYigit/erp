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

namespace ArdaManager.Application.Features.Docs.PurchaseOffers.Commands
{
    public class DeletePurchaseOfferCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    public class DeletePurchaseOfferCommandHandler : IRequestHandler<DeletePurchaseOfferCommand, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;

        public DeletePurchaseOfferCommandHandler(IUnitOfWork<int> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<int>> Handle(DeletePurchaseOfferCommand command, CancellationToken cancellationToken)
        {
            var existingPurchaseOffer = await _unitOfWork.Repository<PurchaseOffer>().GetByIdAsync(command.Id);

            if (existingPurchaseOffer == null)
            {
                return await Result<int>.FailAsync("Satınalma teklifi bulunamadı!");
            }

            // Delete related PurchaseOfferRow records
            var relatedRows = await _unitOfWork.Repository<PurchaseOfferRow>().GetAllAsync(x => x.PurchaseOfferId == command.Id);
            await _unitOfWork.Repository<PurchaseOfferRow>().DeleteRangeAsync(relatedRows);

            await _unitOfWork.Repository<PurchaseOffer>().DeleteAsync(existingPurchaseOffer);
            await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllPurchaseOffersCacheKey);

            return await Result<int>.SuccessAsync(existingPurchaseOffer.Id, "Teklif Silindi!");
        }
    }
}
