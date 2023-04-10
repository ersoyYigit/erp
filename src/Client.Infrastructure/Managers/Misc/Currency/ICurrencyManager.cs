using ArdaManager.Application.Features.Categories.Commands.AddEdit;
using ArdaManager.Application.Features.Categories.Queries.GetAll;
using ArdaManager.Application.Features.Categories.Queries.GetById;
using ArdaManager.Application.Features.Categories.Queries.GetCategoriesByType;
using ArdaManager.Application.Features.Currencies.Commands;
using ArdaManager.Application.Features.Currencies.Queries;
using ArdaManager.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Client.Infrastructure.Managers.Misc.Currency
{
    public interface ICurrencyManager : IManager
    {
        Task<IResult<List<GetAllCurrenciesResponse>>> GetAllCurrenciesAsync();

        Task<IResult<GetEffectiveSellRateResponse>> GetEffectiveSellRate(int currencyId, DateTime? rateDate);
        Task<IResult<List<GetExchangeRatesByDateResponse>>> GetAllRatesByDateAsync(GetExchangeRatesByDateQuery query);
        Task<IResult<int>> UpsertExchangeRatesByDateAsync(UpsertExchangeRatesCommand request);
        Task<IResult<int>> UpsertCurrencyAsync(UpsertCurrencyCommand request);
        Task<IResult<int>> DeleteCurrencyAsync(int id);

        Task<IResult<List<GetExchangeRatesByDateResponse>>> GetLiveRatesByDateAsync(DateTime? date);

    }
}
