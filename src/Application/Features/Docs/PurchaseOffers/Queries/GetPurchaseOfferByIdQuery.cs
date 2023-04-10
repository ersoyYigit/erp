using ArdaManager.Application.Features.Docs.PurchaseRequests.Queries;
using ArdaManager.Application.Interfaces.Repositories;
using ArdaManager.Domain.Entities.Transactions.Purchase;
using ArdaManager.Shared.Wrapper;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static ArdaManager.Shared.Constants.Permission.Permissions;

namespace ArdaManager.Application.Features.Docs.PurchaseOffers.Queries
{
    public class GetPurchaseOfferByIdQuery : IRequest<Result<PurchaseOfferResponse>>
    {
        public int Id { get; set; }
    }

    public class GetPurchaseOfferByIdQueryHandler : IRequestHandler<GetPurchaseOfferByIdQuery, Result<PurchaseOfferResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetPurchaseOfferByIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<PurchaseOfferResponse>> Handle(GetPurchaseOfferByIdQuery query, CancellationToken cancellationToken)
        {
            var purchaseOffer = await _unitOfWork.Repository<PurchaseOffer>().GetByIdAsync(query.Id);

            if (purchaseOffer == null)
            {
                return await Result<PurchaseOfferResponse>.FailAsync("Teklif bulunamadı");
            }

            var purchaseOfferRows = await _unitOfWork.Repository<PurchaseOfferRow>()
                .GetAllAsync(
                    row => row.PurchaseOfferId == query.Id,
                    include: r => r.Include(row => row.MeasurementUnit)
                                  .Include(row => row.Product)
                );

            var purchaseOfferResponse = new PurchaseOfferResponse
            {
                Id = purchaseOffer.Id,
                DocNo = purchaseOffer.DocNo,
                DocDate = purchaseOffer.DocDate,
                Description = purchaseOffer.Description,
                RequesterName = purchaseOffer.RequesterName,
                RequesterId = purchaseOffer.RequesterId,
                RequesterDepartment = purchaseOffer.RequesterDepartment,
                RequirementDate = purchaseOffer.RequirementDate,
                CompanyId = purchaseOffer.CompanyId,
                CompanyName = purchaseOffer.Company.Name,
                PurchaseRequestId = purchaseOffer.PurchaseRequestId,
                PurchaseRequestDocNo = purchaseOffer.PurchaseRequest.DocNo,
                DocType = purchaseOffer.DocType,
                RequesterUserId = purchaseOffer.RequesterId,
                CreatedBy = purchaseOffer.CreatedBy,
                NextDoc = _mapper.Map<BaseDocResponse>(purchaseOffer.NextDoc),
                NextDocId = purchaseOffer.NextDocId,
                PrevDoc = _mapper.Map<BaseDocResponse>(purchaseOffer.PrevDoc),
                PrevDocId = purchaseOffer.PrevDocId,
                Currency = purchaseOffer.Currency,
                CurrencyId = purchaseOffer.CurrencyId,

                CurrencyCode = purchaseOffer.Currency?.Code,
                CurrencyName = purchaseOffer.Currency?.Name,
                ExchangeDate = purchaseOffer.ExchangeDate,
                ExchangeRate = purchaseOffer.ExchangeRate,
                PurchaseOfferRowResponse = purchaseOffer.PurchaseOfferRows
                    .Select(r => new PurchaseOfferRowResponse
                    {
                        Id = r.Id,
                        RowNo = r.RowNo,
                        PurchaseOfferId = r.Id,
                        ProductId = r.ProductId,
                        ProductCode = r.Product.Code,
                        ProductName = r.Product.Name,
                        Quantity = r.Quantity,
                        MeasurementUnitCode = r.MeasurementUnit.Code,
                        MeasurementUnitId = r.MeasurementUnit.Id,
                        MeasurementUnitName = r.MeasurementUnit.Name,
                        Price = r.Price,
                        PurchaseRequestRowId = r.PurchaseRequestRowId,
                        Description = r.Description,
                        Currency = r.Currency,
                        CurrencyId = r.CurrencyId,
                        DiscountAmount = r.DiscountAmount,
                        DiscountPercentage = r.DiscountPercentage,
                        ExchangeRate = r.ExchangeRate,
                        MeasurementUnitSystem = r.MeasurementUnit.System,
                        Tax = r.Tax,
                        TaxAmount = r.TaxAmount,
                        TaxId = r.TaxId,
                        TotalAmount = r.TotalAmount
                    }).ToList()
            };

            return await Result<PurchaseOfferResponse>.SuccessAsync(purchaseOfferResponse);
        }
    }

}
