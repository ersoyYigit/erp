using ArdaManager.Application.Features.Taxes.Commands;
using ArdaManager.Application.Features.Taxes.Queries;
using ArdaManager.Client.Extensions;
using ArdaManager.Client.Infrastructure.Managers.Misc.Tax;
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
    public partial class Taxes
    {
        [Inject] private ITaxManager TaxManager { get; set; }

        [CascadingParameter] private HubConnection HubConnection { get; set; }

        private List<GetAllTaxesResponse> _taxList = new();
        private GetAllTaxesResponse _tax = new();
        private string _searchString = "";


        private ClaimsPrincipal _currentUser;
        private bool _canCreateTaxes;
        private bool _canEditTaxes;
        private bool _canDeleteTaxes;
        //private bool _canExportTaxes;
        private bool _canSearchTaxes;
        private bool _loaded;

        protected override async Task OnInitializedAsync()
        {
            _currentUser = await _authenticationManager.CurrentUser();
            _canCreateTaxes = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Taxes.Create)).Succeeded;
            _canEditTaxes = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Taxes.Edit)).Succeeded;
            _canDeleteTaxes = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Taxes.Delete)).Succeeded;

            await GetTaxesAsync();
            _loaded = true;
            HubConnection = HubConnection.TryInitialize(_navigationManager);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }
        }

        private async Task GetTaxesAsync()
        {
            var response = await TaxManager.GetAllAsync();
            if (response.Succeeded)
            {
                _taxList = response.Data.ToList();
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
                var response = await TaxManager.DeleteAsync(id);
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
                _tax = _taxList.FirstOrDefault(c => c.Id == id);
                if (_tax != null)
                {
                    parameters.Add(nameof(UpsertTaxModal.UpsertTaxModel), new UpsertTaxCommand
                    {
                        Id = _tax.Id,
                        Code = _tax.Code,
                        Name = _tax.Name,
                        Description = _tax.Description,
                        Percent = _tax.Percent,

                    });
                }
            }
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<UpsertTaxModal>(id == 0 ? "Ekle" : "Güncelle", parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                await Reset();
            }
        }

        private async Task Reset()
        {
            _tax = new GetAllTaxesResponse();
            await GetTaxesAsync();
        }
    }
}
