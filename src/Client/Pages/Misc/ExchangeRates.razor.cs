using ArdaManager.Application.Features.Currencies.Commands;
using ArdaManager.Application.Features.Currencies.Queries;
using ArdaManager.Client.Infrastructure.Managers.Misc.Currency;
using ArdaManager.Shared.Wrapper;
using Azure;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ArdaManager.Client.Pages.Misc
{
    public partial class ExchangeRates
    {
        [Inject] private ICurrencyManager CurrencyManager { get; set; }

        private DateTime? SelectedDate { get; set; }
        private List<GetExchangeRatesByDateResponse> _exchangeRates = new();
        private bool ShowImportButton { get; set; } = false;



        private async Task LoadExchangeRates()
        {
            if (SelectedDate.HasValue)
            {
                var result = await CurrencyManager.GetAllRatesByDateAsync(new GetExchangeRatesByDateQuery { RateDate = SelectedDate.Value });
                

                
                if (result.Succeeded && result.Data.Count > 0)
                {
                    _exchangeRates = result.Data;
                    ShowImportButton = false;
                }
                else
                {
                    _exchangeRates.Clear();
                    ShowImportButton = true;
                }
            }
        }

        private async Task ImportExchangeRates()
        {

            if (SelectedDate.HasValue)
            {
                var response = await CurrencyManager.GetLiveRatesByDateAsync(SelectedDate.Value);

                if (response.Succeeded)
                {
                    await LoadExchangeRates();
                }
                else
                {
                    foreach (var message in response.Messages)
                    {
                        _snackBar.Add(message, Severity.Error);
                    }
                }
            }


            
        }
    }
}
