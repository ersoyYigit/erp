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
    public class GetBanknoteSellingQuery : IRequest<Result<GetEffectiveSellRateResponse>>
    {
        public int CurrencyId { get; set; }
        public DateTime? RateDate { get; set; }
    }

    public class GetEffectiveSellRateResponse
    {
        public decimal EffectiveSellRate { get; set; }
        public DateTime RateDate { get; set; }
    }

    internal class GetEffectiveSellRateQueryHandler : IRequestHandler<GetBanknoteSellingQuery, Result<GetEffectiveSellRateResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetEffectiveSellRateQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<GetEffectiveSellRateResponse>> Handle(GetBanknoteSellingQuery request, CancellationToken cancellationToken)
        {
            var rateDate = request.RateDate ?? DateTime.Today.AddDays(-1);

            ExchangeRate exchangeRate = null;

            for (int i = 0; i < 7; i++)
            {
                exchangeRate = await _unitOfWork.Repository<ExchangeRate>()
                    .Entities
                    .SingleOrDefaultAsync(er => er.CurrencyId == request.CurrencyId && er.RateDate == rateDate);

                if (exchangeRate != null)
                    break;

                rateDate = rateDate.AddDays(-1);
            }

            if (exchangeRate == null)
            {
                return await Result<GetEffectiveSellRateResponse>.FailAsync("Döviz kuru bulunamadı.");
            }

            var response = _mapper.Map<GetEffectiveSellRateResponse>(exchangeRate);
            return await Result<GetEffectiveSellRateResponse>.SuccessAsync(response);
        }
    }
}
