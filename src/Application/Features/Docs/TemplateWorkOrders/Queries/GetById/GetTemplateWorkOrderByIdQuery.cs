using ArdaManager.Application.Features.Docs.TemplateWorkOrders.Queries.GetAll;
using ArdaManager.Application.Interfaces.Repositories;
using ArdaManager.Domain.Entities.Transactions;
using ArdaManager.Shared.Constants.Application;
using ArdaManager.Shared.Wrapper;
using AutoMapper;
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

namespace ArdaManager.Application.Features.Docs.TemplateWorkOrders.Queries.GetById
{
    //GetTemplateWorkOrderByIdQuery
    public class GetTemplateWorkOrderByIdQuery : IRequest<Result<GetTemplateWorkOrderByIdResponse>>
    {
        public int Id { get; set; }
    }

    internal class GetTemplateWorkOrderByIdQueryHandler : IRequestHandler<GetTemplateWorkOrderByIdQuery, Result<GetTemplateWorkOrderByIdResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetTemplateWorkOrderByIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<GetTemplateWorkOrderByIdResponse>> Handle(GetTemplateWorkOrderByIdQuery query, CancellationToken cancellationToken)
        {
            Expression<Func<TemplateWorkOrder, GetTemplateWorkOrderByIdResponse>> expression = e => new GetTemplateWorkOrderByIdResponse
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


            GetTemplateWorkOrderByIdResponse getAllTemplateWorkItems = _unitOfWork.Repository<TemplateWorkOrder>().Entities
            .Select(expression).FirstOrDefault(x => x.Id == query.Id);

            return await Result<GetTemplateWorkOrderByIdResponse>.SuccessAsync(getAllTemplateWorkItems);
        }
    }

}
