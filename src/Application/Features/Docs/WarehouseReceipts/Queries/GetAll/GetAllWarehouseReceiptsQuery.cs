using ArdaManager.Application.Features.Docs.WarehouseReceipts.Queries.GetAll;
using ArdaManager.Application.Interfaces.Repositories;
using ArdaManager.Domain.Entities.Transactions;
using ArdaManager.Domain.Entities.Transactions.WarehouseDocs;
using ArdaManager.Domain.Enums.Doc;
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

namespace ArdaManager.Application.Features.Docs.WarehouseReceipts.Queries.GetAll
{
    public class GetAllWarehouseReceiptsQuery : IRequest<Result<List<GetAllWarehouseReceiptsResponse>>>
    {

        public WarehouseReceiptType WarehouseReceiptType { get; set; }
        public GetAllWarehouseReceiptsQuery()
        {
        }
    }

    internal class GetAllWarehouseReceiptsCachedQueryHandler : IRequestHandler<GetAllWarehouseReceiptsQuery, Result<List<GetAllWarehouseReceiptsResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAppCache _cache;

        public GetAllWarehouseReceiptsCachedQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IAppCache cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<Result<List<GetAllWarehouseReceiptsResponse>>> Handle(GetAllWarehouseReceiptsQuery request, CancellationToken cancellationToken)
        {

            Expression<Func<WarehouseReceipt, GetAllWarehouseReceiptsResponse>> expression = e => new GetAllWarehouseReceiptsResponse
            {
                DocNo = e.DocNo,
                DocDate = e.DocDate,
                DocType = e.DocType,
                Id = e.Id,
                Description = e.Description,
                WarehouseId= e.Warehouse.Id,
                WarehouseName = e.Warehouse.Name,
                RelatedCompanyId = e.RelatedCompanyId,
                RelatedCompanyName = e.RelatedCompany.Name,
                RelatedWarehouseId = e.RelatedWarehouse.Id,
                RelatedWarehouseName = e.RelatedWarehouse.Name,
                WarehouseReceiptType = e.WarehouseReceiptType,
                WayBillDate = e.WayBillDate,
                WayBillNo = e.WayBillNo,
                DocState = e.DocState,
                NextDocId = e.NextDocId,
                PrevDocId = e.PrevDocId,
                PurchaseOrderDocNo = e.PurchaseOrderDocNo,
                PurchaseOrderId = e.PurchaseOrderId,
                PurchaseRequestDocNo = e.PurchaseRequest.DocNo,
                PurchaseRequestId = e.PurchaseRequestId,
                RequesterDepartment = e.RequesterDepartment,
                RequesterId = e.RequesterId,
                RequesterName   = e.RequesterName,
                WarehouseOfficerId = e.WarehouseOfficerId,
                WarehouseOfficerName = e.WarehouseOfficerName,
                CreatedBy = e.CreatedBy,


                WarehouseReceiptRowsDto = e.WarehouseReceiptRows
                    .Select(r => new WarehouseReceiptRowsDto
                    {
                            Id = r.Id,
                            ProductId = r.ProductId,
                            ProductName = r.Product.Name,
                            ProductCode = r.Product.Code,
                            Quantity = r.Quantity,
                            MeasurementUnitCode = r.MeasurementUnit.Code,
                            MeasurementUnitId= r.MeasurementUnit.Id,    
                            MeasurementUnitName = r.MeasurementUnit.Name,
                            Price= r.Price,
                            RackCode = r.Rack.Code,
                            RackId = (r.RackId.HasValue) ? r.RackId.Value : 0,
                            RackName = r.Rack.Name,
                            RowNo= r.RowNo,
                            WarehouseReceiptId = r.WarehouseReceiptId,
                            CurrencyCode = r.Currency.Code,
                            CurrencyName = r.Currency.Name,
                            CurrencyId = r.Currency.Id,
                            Description = r.Description,
                            DiscountAmount = r.DiscountAmount,
                            DiscountPercentage = r.DiscountPercentage,
                            ExchangeRate = r.ExchangeRate,
                            PurchaseOrderRowId = r.PurchaseOrderRowId   ,
                            TaxAmount = r.TaxAmount,
                            TaxCode = r.Tax.Code,
                            TaxId = r.TaxId,
                            TotalAmount = r.TotalAmount,
                            MeasurementUnitSystem = r.MeasurementUnit.System
                            // Add other properties you want to select here
                    }) .ToList()

            };

            Func<Task<List<GetAllWarehouseReceiptsResponse>>> getAllTemplateWorkItems = () => _unitOfWork.Repository<WarehouseReceipt>().Entities
                .Select(expression)
                .Where(x=> x.WarehouseReceiptType == request.WarehouseReceiptType)
                .ToListAsync();

            var data = await _cache.GetOrAddAsync(ApplicationConstants.Cache.GetAllWarehouseReceiptsCacheKey + request.WarehouseReceiptType.ToString(), getAllTemplateWorkItems);
            //var mappedWarehouseReceipt = _mapper.Map<List<GetAllWarehouseReceiptsResponse>>(data);

            return await Result<List<GetAllWarehouseReceiptsResponse>>.SuccessAsync(data);

        }
    }
}
