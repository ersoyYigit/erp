using ArdaManager.Application.Interfaces.Repositories;
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

namespace ArdaManager.Application.Features.Docs.WarehouseRequests.Queries
{
    public class GetAllWarehouseRequestsQuery : IRequest<Result<List<WarehouseRequestResponse>>>
    {
        public GetAllWarehouseRequestsQuery()
        {
        }
    }

    internal class GetAllWarehouseRequestsCachedQueryHandler : IRequestHandler<GetAllWarehouseRequestsQuery, Result<List<WarehouseRequestResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAppCache _cache;

        public GetAllWarehouseRequestsCachedQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IAppCache cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<Result<List<WarehouseRequestResponse>>> Handle(GetAllWarehouseRequestsQuery request, CancellationToken cancellationToken)
        {
            _cache.Remove(ApplicationConstants.Cache.GetAllWarehouseRequestsCacheKey);
            Expression<Func<WarehouseRequest, WarehouseRequestResponse>> expression = e => new WarehouseRequestResponse
            {
                DocNo = e.DocNo,
                DocDate = e.DocDate,
                DocType = e.DocType,
                Id = e.Id,
                Description = e.Description,
                WarehouseId = e.Warehouse.Id,
                WarehouseName = e.Warehouse.Name,
                RelatedWarehouseId = e.RelatedWarehouse.Id,
                RelatedWarehouseName = e.RelatedWarehouse.Name,
                DocState = e.DocState,
                NextDocId = e.NextDocId,
                PrevDocId = e.PrevDocId,
                RequesterDepartment = e.RequesterDepartment,
                RequesterId = e.RequesterId,
                RequesterName = e.RequesterName,
                CreatedBy = e.CreatedBy,
                

                WarehouseRequestRowsResponse = e.WarehouseRequestRows
                    .Select(r => new WarehouseRequestRowResponse
                    {
                        Id = r.Id,
                        ProductId = r.ProductId,
                        ProductName = r.Product.Name,
                        ProductCode = r.Product.Code,
                        Quantity = r.Quantity,
                        MeasurementUnitCode = r.MeasurementUnit.Code,
                        MeasurementUnitId = r.MeasurementUnit.Id,
                        MeasurementUnitName = r.MeasurementUnit.Name,
                        MeasurementUnitSystem = r.MeasurementUnit.System,
                        RackCode = r.Rack.Code,
                        RackId = (r.RackId.HasValue) ? r.RackId.Value : 0,
                        RackName = r.Rack.Name,
                        RowNo = r.RowNo,
                        WarehouseRequestId = r.WarehouseRequestId,
                        Description = e.Description
                        // Add other properties you want to select here
                    }).ToList()

            };

            Func<Task<List<WarehouseRequestResponse>>> getAllTemplateWorkItems = () => _unitOfWork.Repository<WarehouseRequest>().Entities
                .Select(expression)
                .ToListAsync();

            var data = await _cache.GetOrAddAsync(ApplicationConstants.Cache.GetAllWarehouseRequestsCacheKey , getAllTemplateWorkItems);
            //var mappedWarehouseRequest = _mapper.Map<List<GetAllWarehouseRequestsResponse>>(data);

            return await Result<List<WarehouseRequestResponse>>.SuccessAsync(data);

        }
    }
}
