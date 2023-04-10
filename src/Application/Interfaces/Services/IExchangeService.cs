using ArdaManager.Application.Features.Currencies.Queries;
using ArdaManager.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Application.Interfaces.Services
{
    public interface IExchangeService
    {
        Task<Result<IEnumerable<GetExchangeRatesByDateResponse>>> GetExchangeRatesByDate(DateTime? CurrencyDate);
    }
}
