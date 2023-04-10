using ArdaManager.Application.Features.Docs.TemplateWorkOrders.Queries.GetAll;
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
using ArdaManager.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.JSInterop;
using ArdaManager.Client.Infrastructure.Managers.Docs.TemplateWorkOrder;
using ArdaManager.Application.Features.Docs.TemplateWorkOrders.Commands.AddEdit;
using ArdaManager.Application.Features.Warehouses.Commands.AddEdit;
using ArdaManager.Client.Pages.Inventory;
using ArdaManager.Domain.Entities.Transactions;
using System.Security.Policy;
using System.Globalization;
using ArdaManager.Domain.Enums.Doc;
using MudBlazor.Extensions.Options;
using MudBlazor.Extensions;

namespace ArdaManager.Client.Pages.Docs.TemplateWorkOrder
{
    public partial class TemplateWorkOrders
    {
        [Inject] private ITemplateWorkOrderManager TemplateWorkOrderManager { get; set; }

        [CascadingParameter] private HubConnection HubConnection { get; set; }

        [Parameter] public int? Id { get; set; }
        [Parameter] public string docno { get; set; }

        private List<GetAllTemplateWorkOrdersResponse> _templateWorkOrderList = new();
        private GetAllTemplateWorkOrdersResponse _templateWorkOrder = new();
        private string _searchString = "";
        private CultureInfo tr = @CultureInfo.GetCultureInfo("tr-TR");

        private ClaimsPrincipal _currentUser;
        private bool _canCreateTemplateWorkOrders;
        private bool _canEditTemplateWorkOrders;
        private bool _canDeleteTemplateWorkOrders;
        private bool _canSearchTemplateWorkOrders;
        private bool _loaded;

        private Dictionary<TemplateWorkOrderState, Color> stateColors = new ();

        protected override async Task OnInitializedAsync()
        {
            _currentUser = await _authenticationManager.CurrentUser();
            _canCreateTemplateWorkOrders = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.TemplateWorkOrders.Create)).Succeeded;
            _canEditTemplateWorkOrders = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.TemplateWorkOrders.Edit)).Succeeded;
            _canDeleteTemplateWorkOrders = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.TemplateWorkOrders.Delete)).Succeeded;
            _canSearchTemplateWorkOrders = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.TemplateWorkOrders.Search)).Succeeded;

            await GetTemplateWorkOrdersAsync();
            _loaded = true;

            stateColors.Add(TemplateWorkOrderState.Waiting, Color.Dark);
            stateColors.Add(TemplateWorkOrderState.Cancelled, Color.Error);
            stateColors.Add(TemplateWorkOrderState.Planned, Color.Info);
            stateColors.Add(TemplateWorkOrderState.Working, Color.Warning);
            stateColors.Add(TemplateWorkOrderState.Done, Color.Success);


            if (Id.HasValue)
            {
                await InvokeModal((int)Id);
            }
            if (!string.IsNullOrWhiteSpace(docno))
            {
                var item = _templateWorkOrderList.Where(x => x.DocNo == docno).FirstOrDefault();
                if (item != null) {
                    await InvokeModal(item.Id);
                }
            }
            HubConnection = HubConnection.TryInitialize(_navigationManager);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }
        }

        private async Task GetTemplateWorkOrdersAsync()
        {
            var response = await TemplateWorkOrderManager.GetAllAsync();
            if (response.Succeeded)
            {
                _templateWorkOrderList = response.Data.ToList();
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
                var response = await TemplateWorkOrderManager.DeleteAsync(id);
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
                _templateWorkOrder = _templateWorkOrderList.FirstOrDefault(c => c.Id == id);
                if (_templateWorkOrder != null)
                {
                    parameters.Add(nameof(AddEditTemplateWorkOrderModal.AddEditTemplateWorkOrderModel), new AddEditTemplateWorkOrderCommand
                    {
                        Id = _templateWorkOrder.Id,
                        TemplateWorkOrderState = _templateWorkOrder.TemplateWorkOrderState, 
                        DocNo = _templateWorkOrder.DocNo,
                        ConsumeProductId = _templateWorkOrder.ConsumeProductId,
                        DocDate = _templateWorkOrder.DocDate,
                        DocType = _templateWorkOrder.DocType,
                        OwnerUserId = _templateWorkOrder.OwnerUserId ,
                        OwnerUserName = _templateWorkOrder.OwnerUserName,
                        PlannedWorkEndDate = _templateWorkOrder.PlannedWorkEndDate,
                        ProductionLineId = _templateWorkOrder.ProductionLineId,
                        ResponseUserId = _templateWorkOrder.ResponseUserId,
                        ResponseUserName= _templateWorkOrder.ResponseUserName,  
                        TemplateWorkOrderType = _templateWorkOrder.TemplateWorkOrderType,
                        WorkEndDate = _templateWorkOrder.WorkEndDate,
                        WorkStartDate = _templateWorkOrder.WorkStartDate,
                        TemplateId = _templateWorkOrder.TemplateId,
                        PlannedWorkStartDate = _templateWorkOrder.PlannedWorkStartDate

                    });
                }
            }
            else
            {
                int max_Id = 1;
                var max_Idrec = _templateWorkOrderList.OrderByDescending(x => x.Id).FirstOrDefault();
                if (max_Idrec != null)
                    max_Id = max_Idrec.Id + 1;

                //string prefix = "KİE";
                string noStr = max_Id.ToString().PadLeft(7, '0');
                //string addedZeroToNoStr = noStr.PadLeft(7, '0');
                string newStr = "KİE" + noStr;

                parameters.Add(nameof(AddEditTemplateWorkOrderModal.AddEditTemplateWorkOrderModel), new AddEditTemplateWorkOrderCommand
                {
                    TemplateWorkOrderState = TemplateWorkOrderState.Waiting,
                    DocDate = DateTime.UtcNow,
                    DocType = DocType.TemplateWorkOrder,
                    DocNo = newStr
                });
            }

            var options = new DialogOptionsEx
            {
                CloseButton = true,
                MaxWidth = MaxWidth.Medium,
                FullWidth = false,
                DisableBackdropClick = true,
                FullHeight = false,
                DragMode = MudDialogDragMode.WithoutBounds,
                Animations = new[] { AnimationType.Pulse },
                Position = DialogPosition.Center,
                Resizeable = false
            };
            //var options = new DialogOptionsEx { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = await _dialogService.ShowEx<AddEditTemplateWorkOrderModal>(id == 0 ? _localizer["Ekle"] : _localizer["Düzenle"], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                await GetTemplateWorkOrdersAsync();
            }
        }

        private async Task Reset()
        {
            _templateWorkOrder = new GetAllTemplateWorkOrdersResponse();
            await GetTemplateWorkOrdersAsync();
        }

        private bool Search(GetAllTemplateWorkOrdersResponse templateWorkOrder)
        {
            if (string.IsNullOrWhiteSpace(_searchString)) return true;
            if (templateWorkOrder.DocNo?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            return false;
        }
    }
}