using ArdaManager.Application.Features.Patterns.Queries.GetAll;
using ArdaManager.Client.Extensions;
using ArdaManager.Shared.Constants.Application;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ArdaManager.Application.Features.Patterns.Commands.AddEdit;
using ArdaManager.Client.Infrastructure.Managers.Catalog.Pattern;
using ArdaManager.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.JSInterop;
using ArdaManager.Client.Infrastructure.Managers.Catalog;
using ArdaManager.Client.Infrastructure.Managers.Corporation.Fair;
using ArdaManager.Application.Features.Fairs.Queries.GetAll;
using ArdaManager.Application.Features.Fairs.Commands.AddEdit;

namespace ArdaManager.Client.Pages.Corporation
{
    public partial class Fairs
    {
        [Inject] private IFairManager FairManager { get; set; }

        [CascadingParameter] private HubConnection HubConnection { get; set; }

        private List<GetAllFairsResponse> _fairList = new();
        private GetAllFairsResponse _fair = new();
        private string _searchString = "";
        //private bool _dense = false;
        //private bool _striped = true;
        //private bool _bordered = false;

        private ClaimsPrincipal _currentUser;
        private bool _canCreateFairs;
        private bool _canEditFairs;
        private bool _canDeleteFairs;
        //private bool _canExportFairs;
        private bool _canSearchFairs;
        private bool _loaded;

        protected override async Task OnInitializedAsync()
        {
            _currentUser = await _authenticationManager.CurrentUser();
            _canCreateFairs = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Companies.Create)).Succeeded;
            _canEditFairs = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Companies.Edit)).Succeeded;
            _canDeleteFairs = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Companies.Delete)).Succeeded;
            _canSearchFairs = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Companies.Search)).Succeeded;

            await GetFairsAsync();
            _loaded = true;
            HubConnection = HubConnection.TryInitialize(_navigationManager);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }
        }

        private async Task GetFairsAsync()
        {
            var response = await FairManager.GetAllAsync();
            if (response.Succeeded)
            {
                _fairList = response.Data.ToList();
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
            string deleteContent = _localizer["Delete Content"];
            var parameters = new DialogParameters
            {
                {nameof(Shared.Dialogs.DeleteConfirmation.ContentText), string.Format(deleteContent, id)}
            };
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<Shared.Dialogs.DeleteConfirmation>(_localizer["Delete"], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                var response = await FairManager.DeleteAsync(id);
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
                _fair = _fairList.FirstOrDefault(c => c.Id == id);
                if (_fair != null)
                {
                    parameters.Add(nameof(AddEditFairModal.AddEditFairModel), new AddEditFairCommand
                    {
                        Id = _fair.Id,
                        Name = _fair.Name,
                        Description = _fair.Description,
                        Code = _fair.Code,
                        Type = _fair.Type,
                        Date = _fair.Date
                    });
                }
            }
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<AddEditFairModal>(id == 0 ? _localizer["Create"] : _localizer["Edit"], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                await Reset();
            }
        }

        private async Task Reset()
        {
            _fair = new GetAllFairsResponse();
            await GetFairsAsync();
        }

        private bool Search(GetAllFairsResponse pattern)
        {
            if (string.IsNullOrWhiteSpace(_searchString)) return true;
            if (pattern.Name?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (pattern.Description?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            return false;
        }
    }
}
