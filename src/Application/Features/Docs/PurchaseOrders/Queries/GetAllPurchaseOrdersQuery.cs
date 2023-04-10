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

namespace ArdaManager.Application.Features.Docs.PurchaseOrders.Queries
{
    public class GetAllPurchaseOrdersQuery : IRequest<Result<List<PurchaseOrderResponse>>>
    {
    }

    public class GetAllPurchaseOrdersQueryHandler : IRequestHandler<GetAllPurchaseOrdersQuery, Result<List<PurchaseOrderResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IAppCache _cache;
        private readonly IMapper _mapper;

        public GetAllPurchaseOrdersQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IAppCache cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<Result<List<PurchaseOrderResponse>>> Handle(GetAllPurchaseOrdersQuery query, CancellationToken cancellationToken)
        {
            _cache.Remove(ApplicationConstants.Cache.GetAllPurchaseOrdersCacheKey);
            Expression<Func<PurchaseOrder, PurchaseOrderResponse>> expression = e => new PurchaseOrderResponse
            {
                Id = e.Id,
                DocNo = e.DocNo,
                DocDate = e.DocDate,
                DocState = e.DocState,
                Description = e.Description,
                RequesterName = e.RequesterName,
                RequesterDepartment = e.RequesterDepartment,
                RequesterId = e.RequesterId,
                RequirementDate = e.RequirementDate,
                CompanyId = e.CompanyId,
                CompanyName = e.Company.Name,
                PurchaseRequestId = e.PurchaseRequestId,
                PurchaseRequestDocNo = e.PurchaseRequest.DocNo,
                PurchaseOfferId = e.PurchaseOfferId,
                PurchaseOfferDocNo = e.PurchaseOffer.DocNo,
                DocType = e.DocType,
                RequesterUserId = e.RequesterId,
                CreatedBy = e.CreatedBy,
                NextDoc = _mapper.Map<BaseDocResponse>(e.NextDoc),
                NextDocId = e.NextDocId,
                PrevDoc = _mapper.Map<BaseDocResponse>(e.PrevDoc),
                PrevDocId = e.PrevDocId,
                Currency = e.Currency,
                CurrencyId = e.CurrencyId,
                CurrencyCode =e.Currency.Code,
                CurrencyName =e.Currency.Name,
                ExchangeDate = e.ExchangeDate,
                ExchangeRate = e.ExchangeRate,
                PurchaseOrderRowResponse = e.PurchaseOrderRows
                    .Select(r => new PurchaseOrderRowResponse
                    {
                        Id = r.Id,
                        PurchaseOrderId = r.PurchaseOrderId,
                        ProductId = r.ProductId,
                        ProductName = r.Product.Name,
                        Quantity = r.Quantity,
                        MeasurementUnitCode = r.MeasurementUnit.Code,
                        MeasurementUnitId = r.MeasurementUnit.Id,
                        MeasurementUnitName = r.MeasurementUnit.Name,
                        MeasurementUnitSystem = r.MeasurementUnit.System,
                        ProductCode = r.Product.Code,
                        RowNo = r.RowNo,
                        PurchaseRequestRowId = r.PurchaseRequestRowId,
                        PurchaseOfferRowId = r.PurchaseOfferRowId,
                        Description = r.Description,
                        Currency = r.Currency,
                        CurrencyId = r.CurrencyId,
                        CurrencyCode = r.Currency.Code,
                        CurrencyName = r.Currency.Name,
                        DiscountAmount = r.DiscountAmount,
                        DiscountPercentage = r.DiscountPercentage,
                        ExchangeRate = r.ExchangeRate,
                        Tax = r.Tax,
                        TaxAmount = r.TaxAmount,
                        TaxCode = r.Tax.Code,
                        Price = r.Price,
                        TaxId = r.TaxId,
                        TotalAmount = r.TotalAmount
                    }).ToList()
            };

            Func<Task<List<PurchaseOrderResponse>>> getAllPurchaseOrders = () => _unitOfWork.Repository<PurchaseOrder>().Entities
                .Select(expression)
                .ToListAsync();

            var data = await _cache.GetOrAddAsync(ApplicationConstants.Cache.GetAllPurchaseOrdersCacheKey, getAllPurchaseOrders);

            return await Result<List<PurchaseOrderResponse>>.SuccessAsync(data);
        }
    }

}
