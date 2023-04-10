using ArdaManager.Application.Features.Docs.WarehouseReceipts.Queries.GetAll;
using ArdaManager.Application.Features.Docs.WarehouseReceipts.Queries.GetById;
using ArdaManager.Application.Interfaces.Repositories;
using ArdaManager.Domain.Entities.Transactions;
using ArdaManager.Domain.Entities.Transactions.WarehouseDocs;
using ArdaManager.Domain.Enums.Doc;
using ArdaManager.Shared.Wrapper;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ArdaManager.Application.Features.Docs.WarehouseReceipts.Queries.GetById
{
    public class GetWarehouseReceiptByIdQuery : IRequest<Result<GetWarehouseReceiptByIdResponse>>
    {
        public int Id { get; set; }
    }

    internal class GetWarehouseReceiptByIdQueryHandler : IRequestHandler<GetWarehouseReceiptByIdQuery, Result<GetWarehouseReceiptByIdResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetWarehouseReceiptByIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<GetWarehouseReceiptByIdResponse>> Handle(GetWarehouseReceiptByIdQuery query, CancellationToken cancellationToken)
        {
            Expression<Func<WarehouseReceipt, GetWarehouseReceiptByIdResponse>> expression = e => new GetWarehouseReceiptByIdResponse
            {
                DocNo = e.DocNo,
                DocDate = e.DocDate,
                DocType = e.DocType,
                Id = e.Id,
                WarehouseId = e.Warehouse.Id,
                WarehouseName = e.Warehouse.Name,
                RelatedCompanyId = e.RelatedCompanyId,
                RelatedCompanyName = e.RelatedCompany.Name,
                RelatedWarehouseId = e.RelatedWarehouse.Id,
                RelatedWarehouseName = e.RelatedWarehouse.Name,
                WarehouseOfficerId = e.Warehouse.Id,
                WarehouseOfficerName = e.Warehouse.Name,
                WarehouseReceipType = e.WarehouseReceiptType,
                WayBillDate = e.WayBillDate,
                WayBillNo = e.WayBillNo,

                WarehouseReceiptRowsDto = e.WarehouseReceiptRows
                    .Select(r => new WarehouseReceiptRowsDto
                    {
                        Id = r.Id,
                        ProductId = r.ProductId,
                        ProductName = $"{r.Product.Code} - {r.Product.Name}",
                        Quantity = r.Quantity,
                        MeasurementUnitCode = r.MeasurementUnit.Code,
                        MeasurementUnitId = r.MeasurementUnit.Id,
                        MeasurementUnitName = r.MeasurementUnit.Name,
                        Price = r.Price,
                        ProductCode = r.Product.Code,
                        RackCode = r.Rack.Code,
                        RackId =  (r.RackId.HasValue) ? r.RackId.Value : 0 ,
                        //RackId =  (r.RackId.HasValue) ? p.ra ,


                        RackName = r.Rack.Name,
                        RowNo = r.RowNo,
                        WarehouseReceiptId = r.WarehouseReceiptId
                        // Add other properties you want to select here
                    }).ToList()


            };


            GetWarehouseReceiptByIdResponse getAllTemplateWorkItems = _unitOfWork.Repository<WarehouseReceipt>().Entities
            .Select(expression).FirstOrDefault(x => x.Id == query.Id);

            return await Result<GetWarehouseReceiptByIdResponse>.SuccessAsync(getAllTemplateWorkItems);
        }
    }

}
