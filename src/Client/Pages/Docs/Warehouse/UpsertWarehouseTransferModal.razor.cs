using ArdaManager.Application.Features.Companies.Queries.Search;
using ArdaManager.Application.Features.Docs.PurchaseOrders.Queries;
using ArdaManager.Application.Features.Docs.WarehouseReceipts.Commands.AddEdit;
using ArdaManager.Application.Features.Docs.WarehouseReceipts.Queries.GetAll;
using ArdaManager.Application.Features.MeasurementUnits.Queries.GetAll;
using ArdaManager.Application.Features.Products.Queries.Search;
using ArdaManager.Application.Features.Racks.Queries.GetAll;
using ArdaManager.Application.Features.Warehouses.Queries.GetAll;
using ArdaManager.Application.Requests.Approval;
using ArdaManager.Application.Responses.Approval;
using ArdaManager.Application.Responses.Identity;
using ArdaManager.Client.Extensions;
using ArdaManager.Client.Infrastructure.Managers.Approval.Scenario;
using ArdaManager.Client.Infrastructure.Managers.Approval;
using ArdaManager.Client.Infrastructure.Managers.Catalog.MeasurementUnit;
using ArdaManager.Client.Infrastructure.Managers.Docs.WarehouseReceipt;
using ArdaManager.Client.Infrastructure.Managers.GlobalVariable;
using ArdaManager.Client.Infrastructure.Managers.Identity.Roles;
using ArdaManager.Client.Infrastructure.Managers.Inventory.Rack;
using ArdaManager.Client.Shared.Components;
using ArdaManager.Client.Shared.Dialogs.RelationPickers.Purchase;
using ArdaManager.Client.Shared.Dialogs;
using ArdaManager.Domain.Enums.Doc;
using ArdaManager.Shared.Constants.Application;
using Blazored.FluentValidation;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor.Extensions.Options;
using MudBlazor.Extensions;
using MudBlazor;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;
using ArdaManager.Client.Shared.Dialogs.RelationPickers.Warehouse;
using ArdaManager.Application.Features.Docs.WarehouseRequests.Queries;
using ArdaManager.Application.Features.Docs.WarehouseRequests.Commands;

namespace ArdaManager.Client.Pages.Docs.Warehouse
{
    public partial class UpsertWarehouseTransferModal
    {
        [Inject] private IWarehouseReceiptManager WarehouseReceiptManager { get; set; }
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

        [Parameter] public AddEditWarehouseReceiptCommand AddEditWarehouseReceiptModel { get; set; } = new();
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
            bool firstTime = (AddEditWarehouseReceiptModel.Id == 0);
            var response = await WarehouseReceiptManager.SaveAsync(AddEditWarehouseReceiptModel);
            if (response.Succeeded)
            {
                _snackBar.Add(response.Messages[0], Severity.Success);

                try
                {


                    if (AddEditWarehouseReceiptModel.PrevDocId.HasValue)
                    {
                        UpdateDocStateRequest closeReq = new UpdateDocStateRequest()
                        {
                            BaseDocId = AddEditWarehouseReceiptModel.PrevDocId.Value,
                            NewDocState = DocState.Finished
                        };
                        await ApproveManager.UpdateDocStateAsync(closeReq);
                    }

                }
                catch (Exception ex)
                {
                    _snackBar.Add(ex.Message, Severity.Error);
                }
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

            if (AddEditWarehouseReceiptModel.WarehouseReceiptRowsDto == null || AddEditWarehouseReceiptModel.WarehouseReceiptRowsDto.Count == 0)
                AddRow();
        }
        private async Task LoadDataAsync()
        {
            var tasks = new List<Task>
            {
                LoadMeasurementUnitsAsync(),
                LoadUsers(),
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
                _scenario = _scenarios.Where(x => x.DocType == AddEditWarehouseReceiptModel.DocType).FirstOrDefault();
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
            AddEditWarehouseReceiptModel.WarehouseReceiptRowsDto ??= new List<WarehouseReceiptRowsDto>();
            AddEditWarehouseReceiptModel.WarehouseReceiptRowsDto.Add(new WarehouseReceiptRowsDto()
            {

                RowNo = AddEditWarehouseReceiptModel.WarehouseReceiptRowsDto.Count + 1,
            });
        }


        private async Task DeleteRow(WarehouseReceiptRowsDto row)
        {
            AddEditWarehouseReceiptModel.WarehouseReceiptRowsDto.Remove(row);
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
                if (AddEditWarehouseReceiptModel.Id == 0)
                {
                    AddEditWarehouseReceiptModel.WarehouseOfficerName = userName;
                    AddEditWarehouseReceiptModel.WarehouseOfficerId = userId;
                    //AddEditWarehouseReceiptModel.RequesterDepartment = _userList.Where(x => x.Id == user.GetUserId()).FirstOrDefault().Department;
                }
            });


        }
        private async Task OpenUserDialogOnF4Code(KeyboardEventArgs e, AddEditWarehouseReceiptCommand context)
        {
            if (e.Code == "F4")
            {
                await OpenUserSearchDialog(context.RequesterName, context);
            }
        }

        private async Task OpenProductSearchDialogOnF4Code(KeyboardEventArgs e, WarehouseReceiptRowsDto rowCtx)
        {
            if (e.Code == "F4")
            {
                await OpenSearchDialog(rowCtx.ProductCode, rowCtx);
            }
        }
        private async Task OpenProductSearchDialogOnF4Name(KeyboardEventArgs e, WarehouseReceiptRowsDto rowCtx)
        {
            if (e.Code == "F4")
            {
                await OpenSearchDialog(rowCtx.ProductName, rowCtx);
            }
        }


        private async Task OpenSearchDialog(string searchStr, WarehouseReceiptRowsDto rowCtx)
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


        private async Task OpenUserSearchDialog(string searchStr, AddEditWarehouseReceiptCommand context)
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
                context.WarehouseOfficerId = selectedUser.Id;
                context.WarehouseOfficerName = selectedUser.FullName;
                //context.RequesterDepartment = selectedUser.Department;


            }
        }



        private async Task<IEnumerable<int?>> SearchRacks(string value)
        {
            // In real life use an asynchronous function for fetching data from an api.
            await Task.Delay(5);

            // if text is null or empty, show complete list
            if (string.IsNullOrEmpty(value))
                return _rackListAll.Select(x => (int?)x.Id);

            return _rackListAll.Where(x => x.Code.Contains(value, StringComparison.InvariantCultureIgnoreCase) && x.WarehouseId == AddEditWarehouseReceiptModel.WarehouseId)
                .Select(x => (int?)x.Id);
        }
        private IEnumerable<string> ValidateRacks(string value)
        {

            if (_rackListAll.Where(x => x.Code == value).Count() == 0)
            {
                yield return "Raf bulunamadı";
            }
        }

        private async Task OpenRequestPickerDialogOnF4Code(KeyboardEventArgs e, AddEditWarehouseReceiptCommand context)
        {
            if (e.Code == "F4")
            {
                await OpenRequestPickerDialog(context.RequesterName, context);
            }
        }
        private async Task OpenRequestPickerDialog(string searchStr, AddEditWarehouseReceiptCommand context)
        {
            var parameters = new DialogParameters();
            parameters.Add("SearchTerm", searchStr);


            var optionsEx = new DialogOptionsEx { CloseButton = true, MaximizeButton = true, MaxWidth = MaxWidth.Large, FullWidth = false, FullScreen = false, DisableBackdropClick = true, FullHeight = false, DragMode = MudDialogDragMode.WithoutBounds, Animations = new[] { AnimationType.Pulse }, Position = MudBlazor.DialogPosition.Center, Resizeable = true };

            var dialog = await _dialogService.ShowEx<WarehouseRequestPicker>("Ara", parameters, optionsEx);

            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                var selectedPurchaseRequest = result.Data as WarehouseRequestResponse;

                FillDataFromPrevDoc(selectedPurchaseRequest);
            }
        }
        private void FillDataFromPrevDoc(WarehouseRequestResponse selectedPurchaseOrder)
        {
            AddEditWarehouseReceiptModel.PrevDocId = selectedPurchaseOrder.Id;
            AddEditWarehouseReceiptModel.PrevDocNo = selectedPurchaseOrder.DocNo;
            AddEditWarehouseReceiptModel.RequesterId = selectedPurchaseOrder.RequesterId;
            AddEditWarehouseReceiptModel.RequesterName = selectedPurchaseOrder.RequesterName;
            AddEditWarehouseReceiptModel.RequesterDepartment = selectedPurchaseOrder.RequesterDepartment;
            //AddEditWarehouseReceiptModel.PurchaseOrderId = selectedPurchaseOrder.Id;
            AddEditWarehouseReceiptModel.RelatedWarehouseId = selectedPurchaseOrder.RelatedWarehouseId;
            AddEditWarehouseReceiptModel.RelatedWarehouseName = selectedPurchaseOrder.RelatedWarehouseName;
            AddEditWarehouseReceiptModel.WarehouseId = selectedPurchaseOrder.WarehouseId;
            AddEditWarehouseReceiptModel.WarehouseName = selectedPurchaseOrder.WarehouseName;

            AddEditWarehouseReceiptModel.WarehouseReceiptRowsDto.Clear();
            foreach (WarehouseRequestRowResponse item in selectedPurchaseOrder.WarehouseRequestRowsResponse)
            {

                AddEditWarehouseReceiptModel.WarehouseReceiptRowsDto.Add(new WarehouseReceiptRowsDto()
                {
                    Description = item.Description,
                    MeasurementUnitCode = item.MeasurementUnitCode,
                    MeasurementUnitId = item.MeasurementUnitId,
                    MeasurementUnitName = item.MeasurementUnitName,
                    MeasurementUnitSystem = item.MeasurementUnitSystem,
                    //Price = item.Price,
                    ProductCode = item.ProductCode,
                    ProductName = item.ProductName,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    RowNo = item.RowNo,
                    //PrevRowId = item.Id,
                    //PurchaseOrderRowId = item.Id,
                    RackId = item.RackId,
                    RackCode = item.RackCode,
                    RackName = item.RackName
                    

                });
            }
            _snackBar.Add("Talep depo giriş fişine dönüştürüldü", Severity.Success, config => { config.ShowTransitionDuration = 5000; });
            //_snackBar.Add("Lütfen teklif aldığınız firmayı seçin.", Severity.Warning, config => { config.ShowTransitionDuration = 5000; });
            //_snackBar.Add("Lütfen ürün fiyatlarını doldurun", Severity.Warning, config => { config.ShowTransitionDuration = 5000; });
        }

        private async Task OpenCompanyDialogOnF4Code(KeyboardEventArgs e, AddEditWarehouseReceiptCommand context)
        {
            if (e.Code == "F4")
            {
                await OpenCompanySearchDialog(context.RequesterName, context);
            }
        }
        private async Task OpenCompanySearchDialog(string searchStr, AddEditWarehouseReceiptCommand context)
        {
            var parameters = new DialogParameters();
            parameters.Add(nameof(CompanySearchDialog.SearchQuery), new CompanySearchQuery
            {
                SearchTerm = searchStr,
                Type = Domain.Enums.CompanyType.Supplier
            });


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

            var dialog = await _dialogService.ShowEx<CompanySearchDialog>("Ara", parameters, optionsEx);

            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                var selectedCompany = result.Data as CompanySearchResultDto;
                context.RelatedCompanyId = selectedCompany.CompanyId;
                context.RelatedCompanyName = selectedCompany.Name;


            }
        }

        private async Task OpenWarehouseDialogOnF4Code(KeyboardEventArgs e, AddEditWarehouseReceiptCommand context)
        {
            if (e.Code == "F4")
            {
                await OpenWarehouseSearchDialog(context.WarehouseName, context);
            }
        }

        private async Task OpenWarehouseSearchDialog(string searchStr, AddEditWarehouseReceiptCommand context)
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


        private async Task OpenWarehouseRelatedDialogOnF4Code(KeyboardEventArgs e, AddEditWarehouseReceiptCommand context)
        {
            if (e.Code == "F4")
            {
                await OpenWarehouseRelatedSearchDialog(context.RelatedWarehouseName, context);
            }
        }

        private async Task OpenWarehouseRelatedSearchDialog(string searchStr, AddEditWarehouseReceiptCommand context)
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
                context.RelatedWarehouseId = selectedWarehouse.Id;
                context.RelatedWarehouseName = selectedWarehouse.Name;

            }
        }
        private async Task ShowQrPop()
        {
            var parameters = new DialogParameters();
            parameters.Add("QrCodeText", $"/catalog/warehouseEntryReceipts/{AddEditWarehouseReceiptModel.Id}");

            var options = new MudBlazor.DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.ExtraSmall };

            var dialog = await _dialogService.ShowAsync<Barcode>("QR Kod", parameters, options);
            var result = await dialog.Result;
        }



    }
}
