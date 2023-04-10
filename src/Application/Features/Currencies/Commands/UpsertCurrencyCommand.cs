using ArdaManager.Application.Interfaces.Repositories;
using ArdaManager.Domain.Entities.Misc;
using ArdaManager.Shared.Constants.Application;
using ArdaManager.Shared.Wrapper;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ArdaManager.Application.Features.Currencies.Commands
{
    public class UpsertCurrencyCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Sign { get; set; }
        public string CustomCode { get; set; }
    }

    public class UpsertCurrencyCommandHandler : IRequestHandler<UpsertCurrencyCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<int> _unitOfWork;

        public UpsertCurrencyCommandHandler(IMapper mapper, IUnitOfWork<int> unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<int>> Handle(UpsertCurrencyCommand command, CancellationToken cancellationToken)
        {
            if (command.Id == 0)
            {
                var currency = _mapper.Map<Currency>(command);
                await _unitOfWork.Repository<Currency>().AddAsync(currency);
                await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllCurrenciesCacheKey);
                return await Result<int>.SuccessAsync(currency.Id, "Para birimi kaydedildi");
            }
            else
            {
                var currency = await _unitOfWork.Repository<Currency>().GetByIdAsync(command.Id);
                if (currency == null)
                {
                    return await Result<int>.FailAsync("Para birimi bulunamadı.");
                }
                currency.Name = command.Name;
                currency.Code = command.Code;
                currency.Sign = command.Sign;
                currency.CustomCode = command.CustomCode;
                await _unitOfWork.Repository<Currency>().UpdateAsync(currency);
                await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllCurrenciesCacheKey);
                return await Result<int>.SuccessAsync(currency.Id, "Para birimi güncellendi");
            }
        }
    }
}
