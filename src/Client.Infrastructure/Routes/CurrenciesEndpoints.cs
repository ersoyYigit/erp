using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Client.Infrastructure.Routes
{
    public static class CurrenciesEndpoints
    {

        public static string GetAll = BaseEndpoint.Server + "api/v1/Currencies/all-currencies";
        public static string UpsertExchangeRatesByDate = BaseEndpoint.Server + "api/v1/Currencies/upsert-exchange-rates";
        public static string GetEffectiveSellingRate(int currencyId, DateTime? date) => BaseEndpoint.Server + $"api/v1/Currencies/effective-sell-rate/{currencyId}?date={date?.ToString("yyyy-MM-dd")}";
        public static string GetAllRatesByDate(DateTime? date) => BaseEndpoint.Server + $"api/v1/Currencies/exchange-rates/{date:yyyy-MM-dd}";
        public static string UpsertCurrency = BaseEndpoint.Server + "api/v1/Currencies/upsert-currency";
        public static string DeleteCurrency(int id) => BaseEndpoint.Server + $"api/v1/Currencies/delete-currency/{id}";


        public static string GetLiveCurrenciesByDate(DateTime? date) => BaseEndpoint.Server + $"api/v1/Currencies/live-currencies/{date:yyyy-MM-dd}";
    }
}
