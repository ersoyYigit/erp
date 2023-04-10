using ArdaManager.Application.Features.Currencies.Commands;
using ArdaManager.Application.Features.Currencies.Queries;
using ArdaManager.Client.Extensions;
using ArdaManager.Client.Infrastructure.Managers.Misc.Currency;
using ArdaManager.Shared.Constants.Application;
using ArdaManager.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ArdaManager.Client.Pages.Misc
{
    public partial class Currencies
    {
        [Inject] private ICurrencyManager CurrencyManager { get; set; }

        [CascadingParameter] private HubConnection HubConnection { get; set; }

        private List<GetAllCurrenciesResponse> _currencyList = new();
        private GetAllCurrenciesResponse _currency = new();
        private string _searchString = "";


        private ClaimsPrincipal _currentUser;
        private bool _canCreateCurrencies;
        private bool _canEditCurrencies;
        private bool _canDeleteCurrencies;
        //private bool _canExportCurrencies;
        private bool _canSearchCurrencies;
        private bool _loaded;

        protected override async Task OnInitializedAsync()
        {
            _currentUser = await _authenticationManager.CurrentUser();
            _canCreateCurrencies = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Currencies.Create)).Succeeded;
            _canEditCurrencies = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Currencies.Edit)).Succeeded;
            _canDeleteCurrencies = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Currencies.Delete)).Succeeded;
            _canSearchCurrencies = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Currencies.Search)).Succeeded;

            await GetCurrenciesAsync();
            _loaded = true;
            HubConnection = HubConnection.TryInitialize(_navigationManager);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }
        }

        private async Task GetCurrenciesAsync()
        {
            var response = await CurrencyManager.GetAllCurrenciesAsync();
            if (response.Succeeded)
            {
                _currencyList = response.Data.ToList();
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }

        private async Task Delete(int id)
        {
            string deleteContent = "Sil";
            var parameters = new DialogParameters
            {
                {nameof(Shared.Dialogs.DeleteConfirmation.ContentText), string.Format(deleteContent, id)}
            };
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<Shared.Dialogs.DeleteConfirmation>("Sil", parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                var response = await CurrencyManager.DeleteCurrencyAsync(id);
                if (response.Succeeded)
                {
                    await Reset();
                    await HubConnection.SendAsync(ApplicationConstants.SignalR.SendUpdateDashboard);
                    _snackBar.Add(response.Messages[0], Severity.Success);
                }
                else
                {
                    await Reset();
                    foreach (var message in response.Messages)
                    {
                        _snackBar.Add(message, Severity.Error);
                    }
                }
            }
        }



        private async Task InvokeModal(int id = 0)
        {
            var parameters = new DialogParameters();
            if (id != 0)
            {
                _currency = _currencyList.FirstOrDefault(c => c.Id == id);
                if (_currency != null)
                {
                    parameters.Add(nameof(UpsertCurrencyModal.UpsertCurrencyModel), new UpsertCurrencyCommand
                    {
                        Id = _currency.Id,
                        Code = _currency.Code,
                        Name = _currency.Name,
                        CustomCode = _currency.CustomCode,
                        Sign = _currency.Sign,

                    });
                }
            }
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<UpsertCurrencyModal>(id == 0 ? "Ekle" : "Güncelle", parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                await Reset();
            }
        }

        private async Task Reset()
        {
            _currency = new GetAllCurrenciesResponse();
            await GetCurrenciesAsync();
        }
    }
}
