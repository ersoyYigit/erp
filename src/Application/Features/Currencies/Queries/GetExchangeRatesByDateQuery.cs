using ArdaManager.Application.Interfaces.Repositories;
using ArdaManager.Domain.Entities.Misc;
using ArdaManager.Shared.Wrapper;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ArdaManager.Application.Features.Currencies.Queries
{
    public class GetExchangeRatesByDateQuery : IRequest<Result<List<GetExchangeRatesByDateResponse>>>
    {
        public DateTime? RateDate { get; set; }
    }

    public class GetExchangeRatesByDateResponse
    {
        public string CurrencyCode { get; set; }
        public string CurrencyName { get; set; }
        public decimal ForexBuying { get; set; }
        public decimal ForexSelling { get; set; }
        public decimal BanknoteBuying { get; set; }
        public decimal BanknoteSelling { get; set; }
    }

    internal class GetExchangeRatesByDateQueryHandler : IRequestHandler<GetExchangeRatesByDateQuery, Result<List<GetExchangeRatesByDateResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetExchangeRatesByDateQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Result<List<GetExchangeRatesByDateResponse>>> Handle(GetExchangeRatesByDateQuery request, CancellationToken cancellationToken)
        {
            var rateDate = request.RateDate ?? DateTime.Today;

            var exchangeRates = await _unitOfWork.Repository<ExchangeRate>()
                .Entities
                .Include(er => er.Currency)
                .Where(er => er.RateDate == rateDate)
                .ToListAsync();

            var mappedExchangeRates = exchangeRates.Select(er => _mapper.Map<GetExchangeRatesByDateResponse>(er)).ToList();
            return await Result<List<GetExchangeRatesByDateResponse>>.SuccessAsync(mappedExchangeRates);
        }
    }
}
