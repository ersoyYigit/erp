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
using ArdaManager.Client.Infrastructure.Managers.Inventory.Warehouse;
using ArdaManager.Application.Features.Warehouses.Queries.GetAll;
using ArdaManager.Application.Features.Warehouses.Commands.AddEdit;
using ArdaManager.Client.Infrastructure.Managers.Inventory.Rack;
using ArdaManager.Application.Features.Racks.Queries.GetAll;
using ArdaManager.Application.Features.Racks.Commands.AddEdit;

namespace ArdaManager.Client.Pages.Inventory
{
    public partial class Racks
    {
        [Inject] private IRackManager RackManager { get; set; }

        [CascadingParameter] private HubConnection HubConnection { get; set; }

        private List<GetAllRacksResponse> _rackList = new();
        private GetAllRacksResponse _rack = new();
        private string _searchString = "";
        
        private ClaimsPrincipal _currentUser;
        private bool _canCreateRacks;
        private bool _canEditRacks;
        private bool _canDeleteRacks;
        private bool _canSearchRacks;
        private bool _loaded;

        protected override async Task OnInitializedAsync()
        {
            _currentUser = await _authenticationManager.CurrentUser();
            _canCreateRacks = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Warehouses.Create)).Succeeded;
            _canEditRacks = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Warehouses.Edit)).Succeeded;
            _canDeleteRacks = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Warehouses.Delete)).Succeeded;
            _canSearchRacks = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Warehouses.Search)).Succeeded;

            await GetRacksAsync();
            _loaded = true;
            HubConnection = HubConnection.TryInitialize(_navigationManager);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }
        }

        private async Task GetRacksAsync()
        {
            var response = await RackManager.GetAllAsync();
            if (response.Succeeded)
            {
                _rackList = response.Data.ToList();
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
                var response = await RackManager.DeleteAsync(id);
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
                _rack = _rackList.FirstOrDefault(c => c.Id == id);
                if (_rack != null)
                {
                    parameters.Add(nameof(AddEditRackModal.AddEditRackModel), new AddEditRackCommand
                    {
                        Id = _rack.Id,
                        Code = _rack.Code,
                        Name = _rack.Name,
                        Section = _rack.Section,
                        SectionCode = _rack.SectionCode,
                        WarehouseId = _rack.WarehouseId,
                        Description = _rack.Description

                    });
                }
            }
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<AddEditRackModal>(id == 0 ? _localizer["Create"] : _localizer["Edit"], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                await Reset();
            }
        }

        private async Task Reset()
        {
            _rack = new GetAllRacksResponse();
            await GetRacksAsync();
        }

        private bool Search(GetAllRacksResponse rack)
        {
            if (string.IsNullOrWhiteSpace(_searchString)) return true;
            if (rack.Name?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (rack.Description?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }

            if (rack.WarehouseName?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            return false;
        }
    }
}
