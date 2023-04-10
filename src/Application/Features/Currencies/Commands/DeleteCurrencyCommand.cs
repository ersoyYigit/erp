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

namespace ArdaManager.Application.Features.Currencies.Commands
{
    public class DeleteCurrencyCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }
    public class DeleteCurrencyCommandHandler : IRequestHandler<DeleteCurrencyCommand, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<DeleteCurrencyCommandHandler> _localizer;

        public DeleteCurrencyCommandHandler(IUnitOfWork<int> unitOfWork, IStringLocalizer<DeleteCurrencyCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(DeleteCurrencyCommand command, CancellationToken cancellationToken)
        {
            var currency = await _unitOfWork.Repository<Currency>().GetByIdAsync(command.Id);
            if (currency != null)
            {
                await _unitOfWork.Repository<Currency>().DeleteAsync(currency);
                await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllCurrenciesCacheKey);
                return await Result<int>.SuccessAsync(currency.Id, _localizer["Para birimi silindi."]);
            }
            else
            {
                return await Result<int>.FailAsync(_localizer["Para birimi bulunamadı."]);
            }
        }
    }
}
