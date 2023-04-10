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

namespace ArdaManager.Client.Pages.Inventory
{
    public partial class Warehouses
    {
        [Inject] private IWarehouseManager WarehouseManager { get; set; }

        [CascadingParameter] private HubConnection HubConnection { get; set; }

        private List<GetAllWarehousesResponse> _warehouseList = new();
        private GetAllWarehousesResponse _warehouse = new();
        private string _searchString = "";
        

        private ClaimsPrincipal _currentUser;
        private bool _canCreateWarehouses;
        private bool _canEditWarehouses;
        private bool _canDeleteWarehouses;
        //private bool _canExportWarehouses;
        private bool _canSearchWarehouses;
        private bool _loaded;

        protected override async Task OnInitializedAsync()
        {
            _currentUser = await _authenticationManager.CurrentUser();
            _canCreateWarehouses = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Warehouses.Create)).Succeeded;
            _canEditWarehouses = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Warehouses.Edit)).Succeeded;
            _canDeleteWarehouses = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Warehouses.Delete)).Succeeded;
            _canSearchWarehouses = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Warehouses.Search)).Succeeded;

            await GetWarehousesAsync();
            _loaded = true;
            HubConnection = HubConnection.TryInitialize(_navigationManager);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }
        }

        private async Task GetWarehousesAsync()
        {
            var response = await WarehouseManager.GetAllAsync();
            if (response.Succeeded)
            {
                _warehouseList = response.Data.ToList();
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
                var response = await WarehouseManager.DeleteAsync(id);
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
                _warehouse = _warehouseList.FirstOrDefault(c => c.Id == id);
                if (_warehouse != null)
                {
                    parameters.Add(nameof(AddEditWarehouseModal.AddEditWarehouseModel), new AddEditWarehouseCommand
                    {
                        Id = _warehouse.Id,
                        Code = _warehouse.Code,
                        Name = _warehouse.Name,
                        Description = _warehouse.Description
                        
                    });
                }
            }
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<AddEditWarehouseModal>(id == 0 ? _localizer["Create"] : _localizer["Edit"], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                await Reset();
            }
        }

        private async Task Reset()
        {
            _warehouse = new GetAllWarehousesResponse();
            await GetWarehousesAsync();
        }

        private bool Search(GetAllWarehousesResponse warehouse)
        {
            if (string.IsNullOrWhiteSpace(_searchString)) return true;
            if (warehouse.Name?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (warehouse.Description?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            return false;
        }
    }
}
