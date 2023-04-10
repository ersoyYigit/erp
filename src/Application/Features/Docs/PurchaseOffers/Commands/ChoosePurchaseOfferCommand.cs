using ArdaManager.Application.Interfaces.Repositories;
using ArdaManager.Domain.Entities.Transactions.Purchase;
using ArdaManager.Shared.Constants.Application;
using ArdaManager.Shared.Wrapper;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ArdaManager.Application.Features.Docs.PurchaseOffers.Commands
{
    public partial class ChoosePurchaseOfferCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    internal class ChoosePurchaseOfferCommandHandler : IRequestHandler<ChoosePurchaseOfferCommand, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;

        public ChoosePurchaseOfferCommandHandler(IUnitOfWork<int> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<int>> Handle(ChoosePurchaseOfferCommand command, CancellationToken cancellationToken)
        {
            // Satınalma teklifi id'si 0 ise hata döndür
            if (command.Id == 0)
            {
                return await Result<int>.FailAsync("Satınalma teklifi bulunamadı!");
            }

            try
            {
                // Mevcut satınalma teklifini al
                var existingPurchaseOffer = await _unitOfWork.Repository<PurchaseOffer>().GetByIdAsync(command.Id);

                // Satınalma teklifi yoksa hata döndür
                if (existingPurchaseOffer == null)
                {
                    return await Result<int>.FailAsync("Satınalma teklifi bulunamadı!");
                }

                // Satınalma teklifi ile ilişikli olan talebin id'sini al
                var requestId = existingPurchaseOffer.PrevDocId;

                // İlişkili talep yoksa hata döndür
                if (requestId == null)
                {
                    return await Result<int>.FailAsync("Satınalma teklifi ile ilişik talep bulunamadı!");
                }

                // Aynı talebe sahip diğer satınalma tekliflerini al
                var purchaseOffers = await _unitOfWork.Repository<PurchaseOffer>().Entities.Where(x => x.PrevDocId == requestId).ToListAsync();

                // Diğer satınalma tekliflerini gezerek durumlarını güncelle
                foreach (var purchaseOffer in purchaseOffers)
                {
                    if (purchaseOffer.Id != existingPurchaseOffer.Id)
                    {
                        purchaseOffer.DocState = Domain.Enums.Doc.DocState.Rejected;
                    }
                    else
                    {
                        purchaseOffer.DocState = Domain.Enums.Doc.DocState.Approved;
                    }
                }

                // Güncellenen tüm satınalma tekliflerini toplu olarak güncelle
                await _unitOfWork.Repository<PurchaseOffer>().UpdateRangeAsync(purchaseOffers);
                await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllPurchaseOffersCacheKey);

                // Başarılı bir şekilde işlem tamamlandığında sonuç döndür
                return await Result<int>.SuccessAsync(existingPurchaseOffer.Id, "Teklif seçimi tamamlandı!");
            }
            catch (Exception ex)
            {
                // Bir hata oluştuğunda hata mesajı döndür
                return await Result<int>.FailAsync(ex.Message);
            }
        }
    }
}
