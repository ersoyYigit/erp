using ArdaManager.Application.Features.Currencies.Commands;
using ArdaManager.Application.Features.Currencies.Queries;
using ArdaManager.Client.Infrastructure.Extensions;
using ArdaManager.Client.Infrastructure.Routes;
using ArdaManager.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Client.Infrastructure.Managers.Misc.Currency
{
    public class CurrencyManager : ICurrencyManager
    {
        private readonly HttpClient _httpClient;

        public CurrencyManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<int>> DeleteCurrencyAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{CurrenciesEndpoints.DeleteCurrency(id)}");
            return await response.ToResult<int>();
        }

        public async Task<IResult<List<GetAllCurrenciesResponse>>> GetAllCurrenciesAsync()
        {
            var response = await _httpClient.GetAsync(CurrenciesEndpoints.GetAll);
            return await response.ToResult<List<GetAllCurrenciesResponse>>();
        }

        public async Task<IResult<List<GetExchangeRatesByDateResponse>>> GetAllRatesByDateAsync(GetExchangeRatesByDateQuery query)
        {
            var response = await _httpClient.GetAsync(CurrenciesEndpoints.GetAllRatesByDate(query.RateDate));
            return await response.ToResult<List<GetExchangeRatesByDateResponse>>();
        }

        public async Task<IResult<GetEffectiveSellRateResponse>> GetEffectiveSellRate(int currencyId, DateTime? rateDate)
        {
            var response = await _httpClient.GetAsync(CurrenciesEndpoints.GetEffectiveSellingRate(currencyId, rateDate));
            return await response.ToResult<GetEffectiveSellRateResponse>();
        }

        public async Task<IResult<int>> UpsertCurrencyAsync(UpsertCurrencyCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(CurrenciesEndpoints.UpsertCurrency, request);
            return await response.ToResult<int>();
        }

        public async Task<IResult<int>> UpsertExchangeRatesByDateAsync(UpsertExchangeRatesCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(CurrenciesEndpoints.UpsertExchangeRatesByDate, request);
            return await response.ToResult<int>();
        }


        public async Task<IResult<List<GetExchangeRatesByDateResponse>>> GetLiveRatesByDateAsync(DateTime? date)
        {
            var response = await _httpClient.GetAsync(CurrenciesEndpoints.GetLiveCurrenciesByDate(date));
            return await response.ToResult<List<GetExchangeRatesByDateResponse>>();
        }
    }
}
