using ArdaManager.Application.Features.Docs.WarehouseRequests.Commands;
using ArdaManager.Application.Features.Docs.WarehouseRequests.Queries;
using ArdaManager.Application.Features.MeasurementUnits.Queries.GetAll;
using ArdaManager.Application.Features.Products.Queries.Search;
using ArdaManager.Application.Features.Racks.Queries.GetAll;
using ArdaManager.Application.Features.Warehouses.Queries.GetAll;
using ArdaManager.Application.Responses.Approval;
using ArdaManager.Application.Responses.Identity;
using ArdaManager.Client.Extensions;
using ArdaManager.Client.Infrastructure.Managers.Approval;
using ArdaManager.Client.Infrastructure.Managers.Approval.Scenario;
using ArdaManager.Client.Infrastructure.Managers.Catalog.MeasurementUnit;
using ArdaManager.Client.Infrastructure.Managers.Docs.WarehouseRequest;
using ArdaManager.Client.Infrastructure.Managers.GlobalVariable;
using ArdaManager.Client.Infrastructure.Managers.Identity.Roles;
using ArdaManager.Client.Infrastructure.Managers.Inventory.Rack;
using ArdaManager.Client.Shared.Components;
using ArdaManager.Client.Shared.Dialogs;
using ArdaManager.Shared.Constants.Application;
using Blazored.FluentValidation;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;
using MudBlazor.Extensions;
using MudBlazor.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArdaManager.Client.Pages.Docs.Warehouse
{
    public partial class UpsertWarehouseRequestModal
    {

        [Inject] private IWarehouseRequestManager WarehouseRequestManager { get; set; }
        [Inject] private IMeasurementUnitManager MeasurementUnitManager { get; set; }
        [Inject] private IRackManager RackManager { get; set; }
        [Inject] private IGlobalVariableService GlobalVarService { get; set; }
        [Inject] private IScenarioManager ScenarioManager { get; set; }
        [Inject] private IRoleManager RoleManager { get; set; }
        [Inject] private IApproveManager ApproveManager { get; set; }


        private List<UserResponse> _userList = new();
        private ApprovalScenarioResponse _scenario = new();
        private List<ApprovalScenarioResponse> _scenarios = new();
        private List<RoleResponse> _roles = new();
        private List<GetAllRacksResponse> _rackList = new();
        private List<GetAllRacksResponse> _rackListAll = new();

        [Parameter] public UpsertWarehouseRequestCommand UpsertWarehouseRequestModel { get; set; } = new();
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }
        [CascadingParameter] private HubConnection HubConnection { get; set; }

        private List<GetAllMeasurementUnitsResponse> _measurementUnits = new();

        private FluentValidationValidator _fluentValidationValidator;
        private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });

        public void Cancel()
        {
            MudDialog.Cancel();
        }

        private async Task SaveAsync()
        {
            bool firstTime = (UpsertWarehouseRequestModel.Id == 0);
            var response = await WarehouseRequestManager.SaveAsync(UpsertWarehouseRequestModel);
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

            if (UpsertWarehouseRequestModel.WarehouseRequestRowsResponse == null || UpsertWarehouseRequestModel.WarehouseRequestRowsResponse.Count == 0)
                AddRow();
        }
        private async Task LoadDataAsync()
        {
            var tasks = new List<Task>
            {
                LoadMeasurementUnitsAsync(),
                SetCurrentUser(),
                GetRacksAsync()
            };

            await Task.WhenAll(tasks);
        }
        private async Task GetRacksAsync()
        {

            var response = await RackManager.GetAllAsync();
            if (response.Succeeded)
            {
                _rackListAll = response.Data.ToList();
                _rackList = _rackListAll;
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }
        private async Task LoadUsers()
        {
            var response = await GlobalVarService.GetAllUsersAsync();
            if (response.Succeeded)
            {
                _userList = response.Data.ToList();
            }
        }
        private async Task LoadRoles()
        {
            var response = await RoleManager.GetRolesAsync();
            if (response.Succeeded)
            {
                _roles = response.Data.ToList();
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }
        private async Task LoadScenario()
        {
            var response = await ScenarioManager.GetAllScenariosAsync();
            if (response.Succeeded)
            {
                _scenarios = response.Data;
            }
            if (_scenarios != null)
            {
                _scenario = _scenarios.Where(x => x.DocType == UpsertWarehouseRequestModel.DocType).FirstOrDefault();
            }

        }

        private async Task LoadMeasurementUnitsAsync()
        {
            //_measurementUnits = GlobalVarService.MeasurementUnits;

            var data = await MeasurementUnitManager.GetAllAsync();
            if (data.Succeeded)
            {
                _measurementUnits = data.Data;
            }

        }


        private void AddRow()
        {
            UpsertWarehouseRequestModel.WarehouseRequestRowsResponse ??= new List<WarehouseRequestRowResponse>();
            UpsertWarehouseRequestModel.WarehouseRequestRowsResponse.Add(new WarehouseRequestRowResponse()
            {

                RowNo = UpsertWarehouseRequestModel.WarehouseRequestRowsResponse.Count + 1,
            });
        }


        private async Task DeleteRow(WarehouseRequestRowResponse row)
        {
            UpsertWarehouseRequestModel.WarehouseRequestRowsResponse.Remove(row);
            await Task.CompletedTask;
        }


        private void OnDescriptionFieldBlur()
        {
            //AddRow();
        }




        private async Task SetCurrentUser()
        {

            var state = await _stateProvider.GetAuthenticationStateAsync();
            var user = state.User;

            var userId = user.GetUserId();
            var userName = user.GetFullName();
            await Task.Run(() =>
            {
                if (UpsertWarehouseRequestModel.Id == 0)
                {
                    UpsertWarehouseRequestModel.RequesterName = userName;
                    UpsertWarehouseRequestModel.RequesterId = userId;
                    UpsertWarehouseRequestModel.RequesterDepartment = _userList.Where(x => x.Id == user.GetUserId()).FirstOrDefault().Department;
                }
            });


        }
        private async Task OpenUserDialogOnF4Code(KeyboardEventArgs e, UpsertWarehouseRequestCommand context)
        {
            if (e.Code == "F4")
            {
                await OpenUserSearchDialog(context.RequesterName, context);
            }
        }

        private async Task OpenProductSearchDialogOnF4Code(KeyboardEventArgs e, WarehouseRequestRowResponse rowCtx)
        {
            if (e.Code == "F4")
            {
                await OpenSearchDialog(rowCtx.ProductCode, rowCtx);
            }
        }
        private async Task OpenProductSearchDialogOnF4Name(KeyboardEventArgs e, WarehouseRequestRowResponse rowCtx)
        {
            if (e.Code == "F4")
            {
                await OpenSearchDialog(rowCtx.ProductName, rowCtx);
            }
        }


        private async Task OpenSearchDialog(string searchStr, WarehouseRequestRowResponse rowCtx)
        {
            var parameters = new DialogParameters();

            parameters.Add("SearchTerm", searchStr);

            //parameters.Add("QrCodeText", $"/catalog/warehouseEntryReceipts/{25}");

            var optionsEx = new DialogOptionsEx
            {
                CloseButton = true,
                MaximizeButton = true,
                MaxWidth = MaxWidth.Large,
                FullWidth = false,
                FullScreen = false,
                DisableBackdropClick = true,
                FullHeight = false,
                DragMode = MudDialogDragMode.WithoutBounds,
                Animations = new[] { AnimationType.Pulse },
                Position = MudBlazor.DialogPosition.Center,
                Resizeable = true
            };
            var options = new MudBlazor.DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.ExtraSmall };

            var dialog = await _dialogService.ShowEx<ProductSearchDialog>("Ara", parameters, optionsEx);
            //var dialog = await _dialogService.ShowAsync<Barcode>("QR Kod", parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                var selectedProduct = result.Data as ProductSearchResultDto;
                rowCtx.ProductId = selectedProduct.ProductId;
                rowCtx.ProductCode = selectedProduct.Code;
                rowCtx.ProductName = selectedProduct.Name;
                rowCtx.MeasurementUnitSystem = selectedProduct.MeasurementUnitSystem;
                rowCtx.MeasurementUnitId = selectedProduct.MeasurementUnitId;


            }
        }


        private async Task OpenUserSearchDialog(string searchStr, UpsertWarehouseRequestCommand context)
        {
            var parameters = new DialogParameters();
            parameters.Add("SearchTerm", searchStr);


            var optionsEx = new DialogOptionsEx
            {
                CloseButton = true,
                MaximizeButton = true,
                MaxWidth = MaxWidth.Large,
                FullWidth = false,
                FullScreen = false,
                DisableBackdropClick = true,
                FullHeight = false,
                DragMode = MudDialogDragMode.WithoutBounds,
                Animations = new[] { AnimationType.Pulse },
                Position = MudBlazor.DialogPosition.Center,
                Resizeable = true
            };

            var dialog = await _dialogService.ShowEx<UserSearchDialog>("Ara", parameters, optionsEx);

            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                var selectedUser = result.Data as UserResponse;
                context.RequesterId = selectedUser.Id;
                context.RequesterName = selectedUser.FullName;
                context.RequesterDepartment = selectedUser.Department;


            }
        }


        private async Task<IEnumerable<int>> SearchRacks(string value)
        {
            // In real life use an asynchronous function for fetching data from an api.
            await Task.Delay(5);

            // if text is null or empty, show complete list
            if (string.IsNullOrEmpty(value))
                return _rackListAll.Select(x => x.Id);

            return _rackListAll.Where(x => x.Code.Contains(value, StringComparison.InvariantCultureIgnoreCase) && x.WarehouseId == UpsertWarehouseRequestModel.WarehouseId)
                .Select(x => x.Id);
        }
        private IEnumerable<string> ValidateRacks(string value)
        {

            if (_rackListAll.Where(x => x.Code == value).Count() == 0)
            {
                yield return "Raf bulunamadı";
            }
        }

        
        private async Task OpenWarehouseDialogOnF4Code(KeyboardEventArgs e, UpsertWarehouseRequestCommand context)
        {
            if (e.Code == "F4")
            {
                await OpenWarehouseSearchDialog(context.WarehouseName, context);
            }
        }

        private async Task OpenWarehouseSearchDialog(string searchStr, UpsertWarehouseRequestCommand context)
        {
            var parameters = new DialogParameters();

            parameters.Add("SearchTerm", searchStr);

            //parameters.Add("QrCodeText", $"/catalog/warehouseEntryReceipts/{25}");

            var optionsEx = new DialogOptionsEx
            {
                CloseButton = true,
                MaximizeButton = true,
                MaxWidth = MaxWidth.Large,
                FullWidth = false,
                FullScreen = false,
                DisableBackdropClick = true,
                FullHeight = false,
                DragMode = MudDialogDragMode.WithoutBounds,
                Animations = new[] { AnimationType.Pulse },
                Position = MudBlazor.DialogPosition.Center,
                Resizeable = true
            };
            var options = new MudBlazor.DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.ExtraSmall };

            var dialog = await _dialogService.ShowEx<WarehouseSearchDialog>("Ara", parameters, optionsEx);
            //var dialog = await _dialogService.ShowAsync<Barcode>("QR Kod", parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                var selectedWarehouse = result.Data as GetAllWarehousesResponse;
                context.WarehouseId = selectedWarehouse.Id;
                context.WarehouseName = selectedWarehouse.Name;

            }
        }


        private async Task ShowQrPop()
        {
            var parameters = new DialogParameters();
            parameters.Add("QrCodeText", $"/catalog/warehouseRequests/{UpsertWarehouseRequestModel.Id}");

            var options = new MudBlazor.DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.ExtraSmall };

            var dialog = await _dialogService.ShowAsync<Barcode>("QR Kod", parameters, options);
            var result = await dialog.Result;
        }



    }
}
