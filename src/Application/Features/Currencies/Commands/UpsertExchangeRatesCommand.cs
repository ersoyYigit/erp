using ArdaManager.Application.Interfaces.Repositories;
using ArdaManager.Domain.Entities.Misc;
using ArdaManager.Shared.Wrapper;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore; // Bu satırı ekleyin


namespace ArdaManager.Application.Features.Currencies.Commands
{
    public class UpsertExchangeRatesCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        public int CurrencyId { get; set; }
        public DateTime RateDate { get; set; }
        public decimal ForexBuying { get; set; }
        public decimal ForexSelling { get; set; }
        public decimal BanknoteBuying { get; set; }
        public decimal BanknoteSelling { get; set; }
    }

    public class UpsertExchangeRatesByDateCommandHandler : IRequestHandler<UpsertExchangeRatesCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<int> _unitOfWork;

        public UpsertExchangeRatesByDateCommandHandler(IMapper mapper, IUnitOfWork<int> unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<int>> Handle(UpsertExchangeRatesCommand command, CancellationToken cancellationToken)
        {
            var existingExchangeRate = await _unitOfWork.Repository<ExchangeRate>()
                .Entities // Bu satırı ekleyin
                .SingleOrDefaultAsync(er => er.CurrencyId == command.CurrencyId && er.RateDate == command.RateDate);


            if (existingExchangeRate == null)
            {
                var exchangeRate = _mapper.Map<ExchangeRate>(command);
                await _unitOfWork.Repository<ExchangeRate>().AddAsync(exchangeRate);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(exchangeRate.Id, "Döviz kuru kaydedildi");
            }
            else
            {
                existingExchangeRate.ForexBuying = command.ForexBuying;
                existingExchangeRate.ForexSelling = command.ForexSelling;
                existingExchangeRate.BanknoteBuying = command.BanknoteBuying;
                existingExchangeRate.BanknoteSelling = command.BanknoteSelling;
                await _unitOfWork.Repository<ExchangeRate>().UpdateAsync(existingExchangeRate);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(existingExchangeRate.Id, "Döviz kuru güncellendi");
            }
        }
    }
}
