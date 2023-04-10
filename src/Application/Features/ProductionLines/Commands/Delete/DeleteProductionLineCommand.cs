using ArdaManager.Application.Interfaces.Repositories;
using ArdaManager.Domain.Entities.Inventory;
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

namespace ArdaManager.Application.Features.ProductionLines.Commands.Delete
{
    public class DeleteProductionLineCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    internal class DeleteProductionLineCommandHandler : IRequestHandler<DeleteProductionLineCommand, Result<int>>
    {

        private readonly IStringLocalizer<DeleteProductionLineCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public DeleteProductionLineCommandHandler(IUnitOfWork<int> unitOfWork, IStringLocalizer<DeleteProductionLineCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(DeleteProductionLineCommand command, CancellationToken cancellationToken)
        {
            if (true)
            {
                var productionLine = await _unitOfWork.Repository<ProductionLine>().GetByIdAsync(command.Id);
                if (productionLine != null)
                {
                    await _unitOfWork.Repository<ProductionLine>().DeleteAsync(productionLine);
                    await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllProductionLinesCacheKey);
                    return await Result<int>.SuccessAsync(productionLine.Id, _localizer["Üretim Hattı Silindi"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["Üretim Hattı bulunamadı!"]);
                }
            }
        }
    }
}
