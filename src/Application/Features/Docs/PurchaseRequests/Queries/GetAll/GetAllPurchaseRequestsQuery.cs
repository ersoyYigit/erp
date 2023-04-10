using ArdaManager.Application.Features.Docs.WarehouseReceipts.Queries.GetAll;
using ArdaManager.Application.Interfaces.Repositories;
using ArdaManager.Domain.Entities.Transactions.Purchase;
using ArdaManager.Domain.Entities.Transactions.WarehouseDocs;
using ArdaManager.Shared.Constants.Application;
using ArdaManager.Shared.Wrapper;
using AutoMapper;
using Azure.Core;
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
using static ArdaManager.Shared.Constants.Application.ApplicationConstants;

namespace ArdaManager.Application.Features.Docs.PurchaseRequests.Queries.GetAll
{
    public class GetAllPurchaseRequestsQuery : IRequest<Result<List<PurchaseRequestDto>>>
    {
    }

    public class GetAllPurchaseRequestsQueryHandler : IRequestHandler<GetAllPurchaseRequestsQuery, Result<List<PurchaseRequestDto>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IAppCache _cache;
        private readonly IMapper _mapper;

        public GetAllPurchaseRequestsQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IAppCache cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<Result<List<PurchaseRequestDto>>> Handle(GetAllPurchaseRequestsQuery query, CancellationToken cancellationToken)
        {

            _cache.Remove(ApplicationConstants.Cache.GetAllPurchaseRequestsCacheKey);

            Expression<Func<PurchaseRequest, PurchaseRequestDto>> expression = e => new PurchaseRequestDto
            {
                DocNo = e.DocNo,
                DocDate = e.DocDate,
                DocType = e.DocType,
                DocState = e.DocState,
                Id = e.Id,
                RequesterId = e.RequesterId,
                RequirementDate= e.RequirementDate,
                Description = e.Description,
                RequesterName = e.RequesterName,
                RequesterDepartment = e.RequesterDepartment,
                RequesterUserId = e.RequesterId,
                CreatedBy = e.CreatedBy,
                //PurchaseRequestRows = e.PurchaseRequestRows,
                PurchaseRequestRowsDto = e.PurchaseRequestRows
                    .Select(r => new PurchaseRequestRowDto
                    {
                        Id = r.Id,
                        ProductId = r.ProductId,
                        ProductName = r.Product.Name,
                        Quantity = r.Quantity,
                        MeasurementUnitCode = r.MeasurementUnit.Code,
                        MeasurementUnitId = r.MeasurementUnit.Id,
                        MeasurementUnitName = r.MeasurementUnit.Name,
                        ProductCode = r.Product.Code,
                        RowNo = r.RowNo,
                        PurchaseRequestId = r.PurchaseRequestId,
                        Description =r.Description
                        // Add other properties you want to select here
                    }).ToList()

            };

            Func<Task<List<PurchaseRequestDto>>> getAllPurchaseRequests = () => _unitOfWork.Repository<PurchaseRequest>().Entities
                .Select(expression)
                .ToListAsync();
            var data = await _cache.GetOrAddAsync(ApplicationConstants.Cache.GetAllPurchaseRequestsCacheKey , getAllPurchaseRequests);
            //var mappedWarehouseReceipt = _mapper.Map<List<GetAllWarehouseReceiptsResponse>>(data);

            return await Result<List<PurchaseRequestDto>>.SuccessAsync(data);


        }
    }
}
