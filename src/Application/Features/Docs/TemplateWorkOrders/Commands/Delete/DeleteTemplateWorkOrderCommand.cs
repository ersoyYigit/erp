using ArdaManager.Application.Interfaces.Repositories;
using ArdaManager.Domain.Entities.Transactions;
using ArdaManager.Shared.Constants.Application;
using ArdaManager.Shared.Wrapper;
using MediatR;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ArdaManager.Application.Features.Docs.TemplateWorkOrders.Commands.Delete
{
    public class DeleteTemplateWorkOrderCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    internal class DeleteTemplateWorkOrderCommandHandler : IRequestHandler<DeleteTemplateWorkOrderCommand, Result<int>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IStringLocalizer<DeleteTemplateWorkOrderCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public DeleteTemplateWorkOrderCommandHandler(IUnitOfWork<int> unitOfWork, IProductRepository productRepository, IStringLocalizer<DeleteTemplateWorkOrderCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _productRepository = productRepository;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(DeleteTemplateWorkOrderCommand command, CancellationToken cancellationToken)
        {

            var TemplateWorkOrder = await _unitOfWork.Repository<TemplateWorkOrder>().GetByIdAsync(command.Id);
            if (TemplateWorkOrder != null)
            {
                await _unitOfWork.Repository<TemplateWorkOrder>().DeleteAsync(TemplateWorkOrder);
                await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllTemplateWorkOrdersCacheKey);
                return await Result<int>.SuccessAsync(TemplateWorkOrder.Id, _localizer["Kalıphane iş emri silindi"]);
            }
            else
            {
                return await Result<int>.FailAsync(_localizer["Kalıphane iş emri bulunamadı!"]);
            }

        }
    }
}
