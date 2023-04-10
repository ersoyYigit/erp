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
using ArdaManager.Client.Infrastructure.Managers.Inventory.ProductionLine;
using ArdaManager.Application.Features.ProductionLines.Queries.GetAll;
using ArdaManager.Application.Features.ProductionLines.Commands.AddEdit;

namespace ArdaManager.Client.Pages.Inventory
{
    public partial class ProductionLines
    {
        [Inject] private IProductionLineManager ProductionLineManager { get; set; }

        [CascadingParameter] private HubConnection HubConnection { get; set; }

        private List<GetAllProductionLinesResponse> _productionLineList = new();
        private GetAllProductionLinesResponse _productionLine = new();
        private string _searchString = "";


        private ClaimsPrincipal _currentUser;
        private bool _canCreateProductionLines;
        private bool _canEditProductionLines;
        private bool _canDeleteProductionLines;
        //private bool _canExportProductionLines;
        private bool _canSearchProductionLines;
        private bool _loaded;

        protected override async Task OnInitializedAsync()
        {
            _currentUser = await _authenticationManager.CurrentUser();
            _canCreateProductionLines = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.ProductionLines.Create)).Succeeded;
            _canEditProductionLines = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.ProductionLines.Edit)).Succeeded;
            _canDeleteProductionLines = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.ProductionLines.Delete)).Succeeded;
            _canSearchProductionLines = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.ProductionLines.Search)).Succeeded;

            await GetProductionLinesAsync();
            _loaded = true;
            HubConnection = HubConnection.TryInitialize(_navigationManager);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }
        }

        private async Task GetProductionLinesAsync()
        {
            var response = await ProductionLineManager.GetAllAsync();
            if (response.Succeeded)
            {
                _productionLineList = response.Data.ToList();
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
                var response = await ProductionLineManager.DeleteAsync(id);
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
                _productionLine = _productionLineList.FirstOrDefault(c => c.Id == id);
                if (_productionLine != null)
                {
                    parameters.Add(nameof(AddEditProductionLineModal.AddEditProductionLineModel), new AddEditProductionLineCommand
                    {
                        Id = _productionLine.Id,
                        Code = _productionLine.Code,
                        Name = _productionLine.Name,
                        Description = _productionLine.Description

                    });
                }
            }
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<AddEditProductionLineModal>(id == 0 ? _localizer["Create"] : _localizer["Edit"], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                await Reset();
            }
        }

        private async Task Reset()
        {
            _productionLine = new GetAllProductionLinesResponse();
            await GetProductionLinesAsync();
        }

        private bool Search(GetAllProductionLinesResponse productionLine)
        {
            if (string.IsNullOrWhiteSpace(_searchString)) return true;
            if (productionLine.Name?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (productionLine.Description?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            return false;
        }
    }
}
