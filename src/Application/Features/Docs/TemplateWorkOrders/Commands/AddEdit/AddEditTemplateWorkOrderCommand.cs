using System;
using System.Threading;
using System.Threading.Tasks;

using ArdaManager.Application.Interfaces.Repositories;
using ArdaManager.Domain.Entities.Transactions;
using ArdaManager.Domain.Enums.Doc;
using ArdaManager.Shared.Constants.Application;
using ArdaManager.Shared.Wrapper;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;

namespace ArdaManager.Application.Features.Docs.TemplateWorkOrders.Commands.AddEdit
{
    public partial class AddEditTemplateWorkOrderCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        public string DocNo { get; set; }
        public DocType DocType { get; set; }
        public TemplateWorkOrderState TemplateWorkOrderState { get; set; }
        public DateTime? DocDate { get; set; }

        public string OwnerUserId { get; set; }
        public string OwnerUserName { get; set; }
        /// <summary>
        /// TODO Nullable olmalı
        /// </summary>
        public string ResponseUserId { get; set; }
        public string ResponseUserName { get; set; }

        public TemplateWorkOrderType TemplateWorkOrderType { get; set; }


        public DateTime? WorkStartDate { get; set; }
        public DateTime? PlannedWorkEndDate { get; set; }
        public DateTime? PlannedWorkStartDate { get; set; }
        public DateTime? WorkEndDate { get; set; }


        public int? ConsumeProductId { get; set; }
        public int? ProductionLineId { get; set; }
        public int TemplateId { get; set; }

    }



    internal class AddEditTemplateWorkOrderCommandHandler : IRequestHandler<AddEditTemplateWorkOrderCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<AddEditTemplateWorkOrderCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public AddEditTemplateWorkOrderCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IStringLocalizer<AddEditTemplateWorkOrderCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(AddEditTemplateWorkOrderCommand command, CancellationToken cancellationToken)
        {
            if (command.Id == 0)
            {
                var templateWorkOrder = _mapper.Map<TemplateWorkOrder>(command);
                await _unitOfWork.Repository<TemplateWorkOrder>().AddAsync(templateWorkOrder);
                await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllTemplateWorkOrdersCacheKey);
                return await Result<int>.SuccessAsync(templateWorkOrder.Id, _localizer["Kalıphane iş emri Kaydedildi"]);
            }
            else
            {
                var templateWorkOrder = await _unitOfWork.Repository<TemplateWorkOrder>().GetByIdAsync(command.Id);
                if (templateWorkOrder != null)
                {
                    templateWorkOrder.DocNo = command.DocNo;
                    templateWorkOrder.DocType = command.DocType;
                    templateWorkOrder.TemplateWorkOrderState = command.TemplateWorkOrderState;
                    templateWorkOrder.DocDate = command.DocDate;
                    templateWorkOrder.OwnerUserId = command.OwnerUserId;
                    templateWorkOrder.OwnerUserName = command.OwnerUserName;
                    templateWorkOrder.ResponseUserId = command.ResponseUserId;
                    templateWorkOrder.ResponseUserName = command.ResponseUserName;
                    templateWorkOrder.TemplateWorkOrderType = command.TemplateWorkOrderType;
                    templateWorkOrder.WorkStartDate = command.WorkStartDate;
                    templateWorkOrder.PlannedWorkEndDate = command.PlannedWorkEndDate;
                    templateWorkOrder.PlannedWorkStartDate = command.PlannedWorkStartDate;
                    templateWorkOrder.WorkEndDate = command.WorkEndDate;
                    templateWorkOrder.ConsumeProductId = command.ConsumeProductId?? templateWorkOrder.ConsumeProductId;
                    templateWorkOrder.ProductionLineId = command.ProductionLineId ?? templateWorkOrder.ProductionLineId;
                    templateWorkOrder.TemplateId = command.TemplateId;





                    await _unitOfWork.Repository<TemplateWorkOrder>().UpdateAsync(templateWorkOrder);
                    await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllTemplateWorkOrdersCacheKey);
                    return await Result<int>.SuccessAsync(templateWorkOrder.Id, _localizer["Kalıphane iş emri güncellendi"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["Kalıphane iş emri bulunamadı!"]);
                }
            }
        }
    }
}
