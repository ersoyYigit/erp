using ArdaManager.Application.Interfaces.Repositories;
using ArdaManager.Domain.Entities.Transactions.Purchase;
using ArdaManager.Shared.Constants.Application;
using ArdaManager.Shared.Wrapper;
using AutoMapper;
using LazyCache;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ArdaManager.Application.Features.Docs.PurchaseOffers.Queries
{
    public class GetAllPurchaseOffersQuery : IRequest<Result<List<PurchaseOfferResponse>>>
    {
    }

    public class GetAllPurchaseOffersQueryHandler : IRequestHandler<GetAllPurchaseOffersQuery, Result<List<PurchaseOfferResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IAppCache _cache;
        private readonly IMapper _mapper;

        public GetAllPurchaseOffersQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IAppCache cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<Result<List<PurchaseOfferResponse>>> Handle(GetAllPurchaseOffersQuery query, CancellationToken cancellationToken)
        {
            _cache.Remove(ApplicationConstants.Cache.GetAllPurchaseOffersCacheKey);
            Expression<Func<PurchaseOffer, PurchaseOfferResponse>> expression = e => new PurchaseOfferResponse
            {
                Id = e.Id,
                DocNo = e.DocNo,
                DocDate = e.DocDate,
                DocState = e.DocState,
                Description = e.Description,
                RequesterName = e.RequesterName,
                RequesterId = e.RequesterId,
                RequesterDepartment = e.RequesterDepartment,
                RequirementDate = e.RequirementDate,
                CompanyId = e.CompanyId,
                CompanyName = e.Company.Name,
                PurchaseRequestId = e.PurchaseRequestId,
                PurchaseRequestDocNo = e.PurchaseRequest.DocNo,
                DocType = e.DocType,
                RequesterUserId = e.RequesterId,
                CreatedBy = e.CreatedBy,
                NextDoc = _mapper.Map<BaseDocResponse>(e.NextDoc),
                NextDocId = e.NextDocId,
                PrevDoc = _mapper.Map<BaseDocResponse>(e.PrevDoc),
                PrevDocId = e.PrevDocId,
                Currency = e.Currency,
                CurrencyId = e.CurrencyId,
                CurrencyCode = e.Currency.Code,
                CurrencyName = e.Currency.Name,
                ExchangeDate = e.ExchangeDate,
                ExchangeRate = e.ExchangeRate,
                PurchaseOfferRowResponse = e.PurchaseOfferRows
                    .Select(r => new PurchaseOfferRowResponse
                    {
                        Id = r.Id,
                        RowNo = r.RowNo,
                        PurchaseOfferId = r.PurchaseOfferId,
                        ProductCode = r.Product.Code,
                        ProductId = r.ProductId,
                        ProductName = r.Product.Name,
                        Quantity = r.Quantity,
                        MeasurementUnitCode = r.MeasurementUnit.Code,
                        MeasurementUnitId = r.MeasurementUnit.Id,
                        MeasurementUnitName = r.MeasurementUnit.Name,
                        Price = r.Price,
                        PurchaseRequestRowId= r.PurchaseRequestRowId,
                        PrevRowId = r.PrevRowId,
                        Description = r.Description,
                        CurrencyId = r.CurrencyId,
                        DiscountAmount = r.DiscountAmount,
                        DiscountPercentage= r.DiscountPercentage,
                        ExchangeRate = r.ExchangeRate,
                        MeasurementUnitSystem = r.MeasurementUnit.System,
                        TaxAmount = r.TaxAmount,
                        TaxId = r.TaxId,
                        TotalAmount = r.TotalAmount
                    }).ToList()
            };

            Func<Task<List<PurchaseOfferResponse>>> getAllPurchaseOffers = () => _unitOfWork.Repository<PurchaseOffer>().Entities
                .Select(expression)
                .ToListAsync();

            var data = await _cache.GetOrAddAsync(ApplicationConstants.Cache.GetAllPurchaseOffersCacheKey, getAllPurchaseOffers);

            return await Result<List<PurchaseOfferResponse>>.SuccessAsync(data);
        }
    }

}
