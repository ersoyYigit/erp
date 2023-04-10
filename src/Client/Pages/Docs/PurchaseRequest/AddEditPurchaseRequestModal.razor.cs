using ArdaManager.Application.Features.Docs.PurchaseRequests.Commands.AddEdit;
using ArdaManager.Application.Features.Products.Queries.Search;
using ArdaManager.Client.Extensions;
using ArdaManager.Client.Infrastructure.Managers.Catalog;
using ArdaManager.Client.Infrastructure.Managers.Catalog.Product;
using ArdaManager.Client.Infrastructure.Managers.Docs.PurcheaseRequest;
using ArdaManager.Client.Shared.Dialogs;
using ArdaManager.Domain.Entities.Transactions.Purchase;
using ArdaManager.Shared.Constants.Application;
using Blazored.FluentValidation;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;
using MudBlazor.Extensions.Options;
using MudBlazor.Extensions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ArdaManager.Domain.Enums.Doc;
using ArdaManager.Client.Shared.Components;
using ArdaManager.Application.Features.Docs.PurchaseRequests.Queries;
using ArdaManager.Client.Infrastructure.Managers.Catalog.MeasurementUnit;
using ArdaManager.Application.Features.MeasurementUnits.Queries.GetAll;
using System.Linq;
using MediatR;
using System.Runtime.CompilerServices;
using FluentValidation.Results;
using ArdaManager.Application.Responses.Identity;
using ArdaManager.Client.Pages.Docs.TemplateWorkOrder;
using ArdaManager.Client.Infrastructure.Managers.GlobalVariable;
using ArdaManager.Client.Infrastructure.Managers.Approval.Scenario;
using ArdaManager.Application.Responses.Approval;
using Nextended.Core.Types;
using ArdaManager.Client.Infrastructure.Managers.Identity.Roles;
using ArdaManager.Client.Pages.Identity.Approval;
using ArdaManager.Application.Interfaces.Approval;
using ArdaManager.Client.Infrastructure.Managers.Approval;
using ArdaManager.Application.Requests.Approval;
using ArdaManager.Client.Infrastructure.Managers.Identity.Users;

namespace ArdaManager.Client.Pages.Docs.PurchaseRequest
{
    public partial class AddEditPurchaseRequestModal
    {
        [Inject] private IPurchaseRequestManager PurchaseRequestManager { get; set; }
        [Inject] private IMeasurementUnitManager MeasurementUnitManager { get; set; }
        [Inject] private IGlobalVariableService GlobalVarService { get; set; }
        [Inject] private IScenarioManager ScenarioManager { get; set; }
        [Inject] private IRoleManager RoleManager { get; set; }
        [Inject] private IApproveManager ApproveManager { get; set; }


        private List<UserResponse> _userList = new();
        private ApprovalScenarioResponse _scenario = new();
        private List<ApprovalScenarioResponse> _scenarios = new();
        private List<RoleResponse> _roles = new();


        [Parameter] public AddEditPurchaseRequestCommand AddEditPurchaseRequestModel { get; set; } = new();
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }
        [CascadingParameter] private HubConnection HubConnection { get; set; }

        private List<GetAllMeasurementUnitsResponse> _measurementUnits = new();

        private FluentValidationValidator _fluentValidationValidator;
        private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });
        private ValidationResult _validationResult;

        public void Cancel()
        {
            MudDialog.Cancel();
        }

        private async Task SaveAsync()
        {
            bool firstTime = (AddEditPurchaseRequestModel.Id == 0);
            var response = await PurchaseRequestManager.SaveAsync(AddEditPurchaseRequestModel);
            if (response.Succeeded)
            {
                if (firstTime)
                {
                    SubmitApprovalRequest request = new SubmitApprovalRequest() {
                        DocType = AddEditPurchaseRequestModel.DocType,
                        DocumentId = response.Data,
                        UserId = AddEditPurchaseRequestModel.RequesterId
                    };

                    var approveResponse = await ApproveManager.SubmitForApprovalAsync(request);
                    if (approveResponse.Succeeded)
                    {
                        _snackBar.Add(approveResponse.Messages[0], Severity.Success);
                        MudDialog.Close();
                    }
                    else { 
                        foreach (var message in approveResponse.Messages)
                        {
                            //_snackBar.Add(message, Severity.Error);
                            MudDialog.Close();
                        }
                    }


                }
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
            
            if (AddEditPurchaseRequestModel.PurchaseRequestRowsDto.Count == 0)
                AddRow();
        }

        private async Task LoadDataAsync()
        {
            await LoadUsers();
            await LoadScenario();
            await LoadRoles();
            await LoadMeasurementUnitsAsync();
            await SetCurrentUser();
            await Task.CompletedTask;
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
                _scenario = _scenarios.Where(x => x.DocType == AddEditPurchaseRequestModel.DocType).FirstOrDefault();
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
            AddEditPurchaseRequestModel.PurchaseRequestRowsDto ??= new List<PurchaseRequestRowDto>();
            AddEditPurchaseRequestModel.PurchaseRequestRowsDto.Add(new PurchaseRequestRowDto() {

                RowNo = AddEditPurchaseRequestModel.PurchaseRequestRowsDto.Count + 1,
            });
        }
       
        
        private async Task DeleteRow(PurchaseRequestRowDto row)
        {
            AddEditPurchaseRequestModel.PurchaseRequestRowsDto.Remove(row);
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
                if (AddEditPurchaseRequestModel.Id == 0)
                {
                    AddEditPurchaseRequestModel.RequesterName = userName;
                    AddEditPurchaseRequestModel.RequesterId = userId;
                    AddEditPurchaseRequestModel.RequesterDepartment = _userList.Where(x=> x.Id == user.GetUserId()).FirstOrDefault().Department;
                }
            });


        }
        private async Task OpenUserDialogOnF4Code(KeyboardEventArgs e, AddEditPurchaseRequestCommand context)
        {
            if ( e.Code == "F4")
            {
                await OpenUserSearchDialog(context.RequesterName, context);
            }
        }

        private async Task OpenProductSearchDialogOnF4Code(KeyboardEventArgs e, PurchaseRequestRowDto rowCtx)
        {
            if ( e.Code == "F4")
            {
                await OpenSearchDialog(rowCtx.ProductCode, rowCtx);
            }
        }
        private async Task OpenProductSearchDialogOnF4Name(KeyboardEventArgs e, PurchaseRequestRowDto rowCtx)
        {
            if (e.Code == "F4")
            {
                await OpenSearchDialog(rowCtx.ProductName, rowCtx);
            }
        }


        private async Task OpenSearchDialog(string searchStr, PurchaseRequestRowDto rowCtx)
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
                Position = DialogPosition.Center,
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


        private async Task OpenUserSearchDialog(string searchStr, AddEditPurchaseRequestCommand context)
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
                Position = DialogPosition.Center,
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

    }
}
