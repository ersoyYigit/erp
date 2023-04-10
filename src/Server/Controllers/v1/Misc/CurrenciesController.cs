using ArdaManager.Application.Features.Currencies.Commands;
using ArdaManager.Application.Features.Currencies.Queries;
using ArdaManager.Application.Interfaces.Services;
using ArdaManager.Infrastructure.Services;
using ArdaManager.Server.Services;
using ArdaManager.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Polly;
using System;
using System.Threading.Tasks;

namespace ArdaManager.Server.Controllers.v1.Misc
{
    public class CurrenciesController : BaseApiController<CurrenciesController>
    {
        private readonly IExchangeService _exchangeService;
        public CurrenciesController(IExchangeService exchangeService)
        {
            _exchangeService = exchangeService;
        }


        [HttpGet("effective-sell-rate/{currencyId}/{rateDate?}")]
        public async Task<IActionResult> GetEffectiveSellRate(int currencyId, DateTime? rateDate)
        {
            var result = await _mediator.Send(new GetBanknoteSellingQuery { CurrencyId = currencyId, RateDate = rateDate });
            return Ok(result);
        }

        [HttpGet("exchange-rates/{rateDate?}")]
        public async Task<IActionResult> GetExchangeRatesByDate(DateTime? rateDate)
        {
            var result = await _mediator.Send(new GetExchangeRatesByDateQuery { RateDate = rateDate });
            return Ok(result);
        }

        [HttpPost("upsert-exchange-rates")]
        [Authorize(Policy = Permissions.Currencies.Edit)]
        public async Task<IActionResult> UpsertExchangeRatesByDate(UpsertExchangeRatesCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }




        [HttpPost("upsert-currency")]
        [Authorize(Policy = Permissions.Currencies.Edit)]
        public async Task<IActionResult> UpsertCurrency(UpsertCurrencyCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("delete-currency/{id}")]
        [Authorize(Policy = Permissions.Currencies.Delete)]
        public async Task<IActionResult> DeleteCurrency(int id)
        {
            var result = await _mediator.Send(new DeleteCurrencyCommand { Id = id });
            return Ok(result);
        }

        [HttpGet("all-currencies")]
        public async Task<IActionResult> GetAllCurrencies()
        {
            var result = await _mediator.Send(new GetAllCurrenciesQuery());
            return Ok(result);
        }



        [HttpGet("live-currencies/{date?}")]
        public async Task<IActionResult> GetLiveCurrencies(DateTime? date)
        {
            return Ok(await _exchangeService.GetExchangeRatesByDate(date));
        }

    }
}
