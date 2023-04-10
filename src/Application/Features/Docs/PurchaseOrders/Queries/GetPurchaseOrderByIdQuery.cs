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

namespace ArdaManager.Application.Features.Docs.PurchaseOrders.Queries
{
    public class GetPurchaseOrderByIdQuery : IRequest<Result<PurchaseOrderResponse>>
    {
        public int Id { get; set; }
    }

    public class GetPurchaseOrderByIdQueryHandler : IRequestHandler<GetPurchaseOrderByIdQuery, Result<PurchaseOrderResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetPurchaseOrderByIdQueryHandler(IUnitOfWork<int> unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<PurchaseOrderResponse>> Handle(GetPurchaseOrderByIdQuery query, CancellationToken cancellationToken)
        {
            var purchaseOrder = await _unitOfWork.Repository<PurchaseOrder>().GetByIdAsync(query.Id);

            if (purchaseOrder == null)
            {
                return await Result<PurchaseOrderResponse>.FailAsync("Sipariş bulunamadı");
            }

            var purchaseOrderRows = await _unitOfWork.Repository<PurchaseOrderRow>()
                .GetAllAsync(
                    row => row.PurchaseOrderId == query.Id,
                    include: r => r.Include(row => row.MeasurementUnit)
                                  .Include(row => row.Product)
                );

            var purchaseOrderResponse = new PurchaseOrderResponse
            {
                Id = purchaseOrder.Id,
                DocNo = purchaseOrder.DocNo,
                DocDate = purchaseOrder.DocDate,
                Description = purchaseOrder.Description,
                RequesterName = purchaseOrder.RequesterName,
                RequesterDepartment = purchaseOrder.RequesterDepartment,
                RequesterId = purchaseOrder.RequesterId,
                RequirementDate = purchaseOrder.RequirementDate,
                CompanyId = purchaseOrder.CompanyId,
                CompanyName = purchaseOrder.Company.Name,
                PurchaseRequestId = purchaseOrder.PurchaseRequestId,
                PurchaseRequestDocNo = purchaseOrder.PurchaseRequest.DocNo,
                PurchaseOfferId = purchaseOrder.PurchaseOfferId,
                PurchaseOfferDocNo = purchaseOrder.PurchaseOffer.DocNo,
                DocType = purchaseOrder.DocType,
                RequesterUserId = purchaseOrder.RequesterId,
                CreatedBy = purchaseOrder.CreatedBy,
                NextDoc = _mapper.Map<BaseDocResponse>(purchaseOrder.NextDoc),
                NextDocId = purchaseOrder.NextDocId,
                PrevDoc = _mapper.Map<BaseDocResponse>(purchaseOrder.PrevDoc),
                PrevDocId = purchaseOrder.PrevDocId,
                Currency = purchaseOrder.Currency,
                CurrencyId = purchaseOrder.CurrencyId,

                CurrencyCode = purchaseOrder.Currency?.Code,
                CurrencyName = purchaseOrder.Currency?.Name,
                ExchangeDate = purchaseOrder.ExchangeDate,
                ExchangeRate = purchaseOrder.ExchangeRate,
                PurchaseOrderRowResponse = purchaseOrder.PurchaseOrderRows
                    .Select(r => new PurchaseOrderRowResponse
                    {
                        Id = r.Id,
                        PurchaseOrderId = r.Id,
                        ProductId = r.ProductId,
                        ProductName = r.Product.Name,
                        Quantity = r.Quantity,
                        MeasurementUnitCode = r.MeasurementUnit.Code,
                        MeasurementUnitId = r.MeasurementUnit.Id,
                        MeasurementUnitName = r.MeasurementUnit.Name,
                        ProductCode = r.Product.Code,
                        RowNo = r.RowNo,
                        PurchaseRequestRowId = r.PurchaseRequestRowId,
                        PurchaseOfferRowId = r.PurchaseOfferRowId,
                        Description = r.Description,
                        Currency = r.Currency,
                        CurrencyId = r.CurrencyId,
                        DiscountAmount = r.DiscountAmount,
                        DiscountPercentage = r.DiscountPercentage,
                        ExchangeRate = r.ExchangeRate,
                        Tax = r.Tax,
                        TaxAmount = r.TaxAmount,
                        TaxId = r.TaxId,
                        TotalAmount = r.TotalAmount
                    }).ToList()
            };

            return await Result<PurchaseOrderResponse>.SuccessAsync(purchaseOrderResponse);
        }
    }

}
