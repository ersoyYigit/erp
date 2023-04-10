using ArdaManager.Application.Interfaces.Repositories;
using ArdaManager.Domain.Entities.Misc;
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

namespace ArdaManager.Application.Features.Taxes.Commands
{
    public class DeleteTaxCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }
    public class DeleteTaxCommandHandler : IRequestHandler<DeleteTaxCommand, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<DeleteTaxCommandHandler> _localizer;

        public DeleteTaxCommandHandler(IUnitOfWork<int> unitOfWork, IStringLocalizer<DeleteTaxCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(DeleteTaxCommand command, CancellationToken cancellationToken)
        {
            var currency = await _unitOfWork.Repository<Tax>().GetByIdAsync(command.Id);
            if (currency != null)
            {
                await _unitOfWork.Repository<Tax>().DeleteAsync(currency);
                await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllTaxesCacheKey);
                return await Result<int>.SuccessAsync(currency.Id, _localizer["Vergi dilimi silindi."]);
            }
            else
            {
                return await Result<int>.FailAsync(_localizer["Vergi dilimi bulunamadı."]);
            }
        }
    }
}
