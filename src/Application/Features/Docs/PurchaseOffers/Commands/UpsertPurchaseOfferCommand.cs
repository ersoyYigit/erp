using ArdaManager.Application.Features.Docs.PurchaseOffers.Queries;
using ArdaManager.Application.Interfaces.Repositories;
using ArdaManager.Domain.Entities.Transactions.Purchase;
using ArdaManager.Domain.Enums.Doc;
using ArdaManager.Shared.Constants.Application;
using ArdaManager.Shared.Wrapper;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ArdaManager.Application.Features.Docs.PurchaseOffers.Commands
{
    public partial class UpsertPurchaseOfferCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        public string DocNo { get; set; }
        public DocType DocType { get; set; }
        public DateTime? DocDate { get; set; }
        public string Description { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string RequesterId { get; set; }
        public string RequesterName { get; set; }
        public string RequesterDepartment { get; set; }
        public int? PurchaseRequestId { get; set; }
        public int? PrevDocId { get; set; }
        public string PrevDocNo { get; set; }
        public int? NextDocId { get; set; }
        public string NextDocNo { get; set; }
        public int CurrencyId { get; set; }
        public string CurrencyCode { get; set; }
        public string CurrencyName { get; set; }
        public DateTime ExchangeDate { get; set; }
        public decimal ExchangeRate { get; set; }

        public ICollection<PurchaseOfferRowResponse> PurchaseOfferRowResponse { get; set; }
    }

    internal class UpsertPurchaseOfferCommandHandler : IRequestHandler<UpsertPurchaseOfferCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<UpsertPurchaseOfferCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public UpsertPurchaseOfferCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IStringLocalizer<UpsertPurchaseOfferCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(UpsertPurchaseOfferCommand command, CancellationToken cancellationToken)
        {
            if (command.Id == 0)
            {
                try
                {
                    var purchaseOffer = _mapper.Map<PurchaseOffer>(command);
                    purchaseOffer.PurchaseOfferRows = _mapper.Map<ICollection<PurchaseOfferRow>>(command.PurchaseOfferRowResponse);

                    await _unitOfWork.Repository<PurchaseOffer>().AddAsync(purchaseOffer);
                    await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllPurchaseOffersCacheKey);
                    return await Result<int>.SuccessAsync(purchaseOffer.Id, "Teklif kaydedildi!");
                }
                catch (Exception ex)
                {
                    return await Result<int>.FailAsync(ex.Message);
                }
            }
            else
            {
                try
                {
                    var existingPurchaseOffer = await _unitOfWork.Repository<PurchaseOffer>().GetByIdAsync(command.Id);

                    if (existingPurchaseOffer == null)
                    {
                        return await Result<int>.FailAsync(_localizer["Satınalma teklifi bulunamadı!"]);
                    }

                    _mapper.Map(command, existingPurchaseOffer);
                    existingPurchaseOffer.PurchaseOfferRows.Clear();
                    existingPurchaseOffer.PurchaseOfferRows = _mapper.Map<ICollection<PurchaseOfferRow>>(command.PurchaseOfferRowResponse);

                    await _unitOfWork.Repository<PurchaseOffer>().UpdateAsync(existingPurchaseOffer);
                    await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllPurchaseOffersCacheKey);

                    return await Result<int>.SuccessAsync(existingPurchaseOffer.Id, "Teklif güncellendi!");
                }
                catch (Exception ex)
                {
                    return await Result<int>.FailAsync(ex.Message);
                }
            }
        }
    }
}
