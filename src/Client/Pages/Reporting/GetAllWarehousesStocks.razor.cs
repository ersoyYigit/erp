using ArdaManager.Shared.Constants.Permission;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System;
using ArdaManager.Domain.Entities.Report.Warehouse;
using ArdaManager.Client.Infrastructure.Managers.Reporting;
using Microsoft.AspNetCore.Authorization;
using ArdaManager.Client.Extensions;
using ArdaManager.Application.Features.Reports.Warehouses;
using System.Linq;
using Radzen.Blazor;
using Radzen;
using static MudBlazor.CategoryTypes;

namespace ArdaManager.Client.Pages.Reporting
{
    partial class GetAllWarehousesStocks
    {
        [Inject] private IReportsManager ReportsManager { get; set; }

        [CascadingParameter] private HubConnection HubConnection { get; set; }
        [Parameter] public int? Id { get; set; }

        private List<WarehouseReport> _resultList = new();
        private WarehouseReport _result = new();
        private MudDataGrid<WarehouseReport> mudGrid;
        private string _searchString = "";
        bool _loaded;

        private ClaimsPrincipal _currentUser;
        private bool _canViewGetAllWarehousesStocks;

        protected override async Task OnInitializedAsync()
        {
            _loaded = false;
            _currentUser = await _authenticationManager.CurrentUser();
            _canViewGetAllWarehousesStocks = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Reports.GetAllWarehousesStocks)).Succeeded;

            await GetReportResults();
            _loaded = true;

            await InvokeAsync(StateHasChanged);

        }

        private async Task GetReportResults()
        {
            
            GetAllWarehousesStocksQuery query = new GetAllWarehousesStocksQuery() { WarehouseId = ((Id.HasValue) ? Id.Value: 0 ) };
            var response = await ReportsManager.GetAllWarehousesStocks(query);
            if (response.Succeeded)
            {
                _resultList = response.Data.ToList();
                //await InvokeAsync(StateHasChanged);
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }

        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
                Console.WriteLine(mudGrid.FilterDefinitions.First().Title);
    }

        /*
        private async Task ExportToExcel()
        {
            var response = await PatternManager.ExportToExcelAsync(_searchString);
            if (response.Succeeded)
            {
                await _jsRuntime.InvokeVoidAsync("Download", new
                {
                    ByteArray = response.Data,
                    FileName = $"{nameof(Patterns).ToLower()}_{DateTime.Now:ddMMyyyyHHmmss}.xlsx",
                    MimeType = ApplicationConstants.MimeTypes.OpenXml
                });
                _snackBar.Add(string.IsNullOrWhiteSpace(_searchString)
                    ? _localizer["Patterns exported"]
                    : _localizer["Filtered Patterns exported"], Severity.Success);
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }
        */



        private Func<WarehouseReport, bool> _quickFilter => x =>
        {
            if (string.IsNullOrWhiteSpace(_searchString)) return true;
            if (x.WarehouseName?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (x.ProductCode?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (x.ProductName?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (x.WarehouseCode?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (x.CategoryName?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            return false;
        };






    }
}
