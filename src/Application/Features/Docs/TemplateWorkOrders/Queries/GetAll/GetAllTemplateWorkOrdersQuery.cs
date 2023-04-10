
using ArdaManager.Application.Extensions;
using ArdaManager.Application.Features.Patterns.Queries.GetAll;
using ArdaManager.Application.Features.Products.Queries.GetAllPaged;
using ArdaManager.Application.Interfaces.Repositories;
using ArdaManager.Domain.Entities.Catalog;
using ArdaManager.Domain.Entities.Transactions;
using ArdaManager.Domain.Enums;
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

namespace ArdaManager.Application.Features.Docs.TemplateWorkOrders.Queries.GetAll
{
    
    public class GetAllTemplateWorkOrdersQuery : IRequest<Result<List<GetAllTemplateWorkOrdersResponse>>>
    {
        public GetAllTemplateWorkOrdersQuery()
        {
        }
    }

    internal class GetAllTemplateWorkOrdersCachedQueryHandler : IRequestHandler<GetAllTemplateWorkOrdersQuery, Result<List<GetAllTemplateWorkOrdersResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAppCache _cache;

        public GetAllTemplateWorkOrdersCachedQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IAppCache cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<Result<List<GetAllTemplateWorkOrdersResponse>>> Handle(GetAllTemplateWorkOrdersQuery request, CancellationToken cancellationToken)
        {

            Expression<Func<TemplateWorkOrder, GetAllTemplateWorkOrdersResponse>> expression = e => new GetAllTemplateWorkOrdersResponse
            {
                DocNo = e.DocNo,
                ConsumeProductCode = e.ConsumeProduct.Code,
                ConsumeProductId = e.ConsumeProductId,
                ConsumeProductName = e.ConsumeProduct.Name,
                DocDate = e.DocDate,
                TemplateWorkOrderState = e.TemplateWorkOrderState,
                DocType = e.DocType,
                Id = e.Id,
                OwnerUserId = e.OwnerUserId,
                OwnerUserName = e.OwnerUserName,
                PlannedWorkEndDate = e.PlannedWorkEndDate,
                PlannedWorkStartDate = e.PlannedWorkStartDate,
                ProductionLineCode = e.ProductionLine.Code,
                ProductionLineId = e.ProductionLineId,
                ProductionLineName = e.ProductionLine.Name,
                ResponseUserId = e.ResponseUserId,
                ResponseUserName = e.ResponseUserName,
                TemplateWorkOrderType = e.TemplateWorkOrderType,
                WorkEndDate = e.WorkEndDate,
                WorkStartDate = e.WorkStartDate,
                TemplateId = e.TemplateId,
                TemplateCode = e.Template.Code,
                TemplateName = e.Template.Name

                
            };

            Func<Task<List<GetAllTemplateWorkOrdersResponse>>> getAllTemplateWorkItems = () => _unitOfWork.Repository<TemplateWorkOrder>().Entities
                .Select(expression)
                .ToListAsync();

            var data = await _cache.GetOrAddAsync(ApplicationConstants.Cache.GetAllTemplateWorkOrdersCacheKey, getAllTemplateWorkItems);
            //var mappedTemplateWorkOrder = _mapper.Map<List<GetAllTemplateWorkOrdersResponse>>(data);

            return await Result<List<GetAllTemplateWorkOrdersResponse>>.SuccessAsync(data);

        }
    }
}
