using ArdaManager.Application.Features.Categories.Queries.GetCategoriesByType;
using ArdaManager.Application.Features.Racks.Commands.AddEdit;
using ArdaManager.Application.Features.Warehouses.Queries.GetAll;
using ArdaManager.Client.Extensions;
using ArdaManager.Client.Infrastructure.Managers.Inventory.Rack;
using ArdaManager.Client.Infrastructure.Managers.Inventory.Warehouse;
using ArdaManager.Shared.Constants.Application;
using Blazored.FluentValidation;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArdaManager.Client.Pages.Inventory
{
    public partial class AddEditRackModal
    {
        [Inject] private IRackManager RackManager { get; set; }
        [Inject] private IWarehouseManager WarehouseManager { get; set; }

        [Parameter] public AddEditRackCommand AddEditRackModel { get; set; } = new();
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }
        [CascadingParameter] private HubConnection HubConnection { get; set; }

        private FluentValidationValidator _fluentValidationValidator;
        private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });
        private List<GetAllWarehousesResponse> _warehouses = new();
        

        public void Cancel()
        {
            MudDialog.Cancel();
        }

        private async Task SaveAsync()
        {
            var response = await RackManager.SaveAsync(AddEditRackModel);
            if (response.Succeeded)
            {
                _snackBar.Add(response.Messages[0], Severity.Success);
                MudDialog.Close();
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
            await HubConnection.SendAsync(ApplicationConstants.SignalR.SendUpdateDashboard);
        }

        protected override async Task OnInitializedAsync()
        {
            await LoadDataAsync();
            HubConnection = HubConnection.TryInitialize(_navigationManager);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }
        }

        private async Task LoadDataAsync()
        {
            await LoadWarehousesAsync();
            await Task.CompletedTask;
        }

        private async Task LoadWarehousesAsync()
        {
            var data = await WarehouseManager.GetAllAsync();
            if (data.Succeeded)
            {
                _warehouses = data.Data;
            }
        }

        private async Task<IEnumerable<int>> SearchWarehouses(string value)
        {
            // In real life use an asynchronous function for fetching data from an api.
            await Task.Delay(5);

            // if text is null or empty, show complete list
            if (string.IsNullOrEmpty(value))
                return _warehouses.Select(x => x.Id);

            return _warehouses.Where(x => x.Name.Contains(value, StringComparison.InvariantCultureIgnoreCase))
                .Select(x => x.Id);
        }

    }
}
