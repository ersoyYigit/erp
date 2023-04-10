using ArdaManager.Client.Infrastructure.Managers.Docs.PurchaseOrder;
using ArdaManager.Shared.Constants.Application;
using ArdaManager.Shared.Constants.Permission;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;
using MudBlazor.Extensions.Options;
using MudBlazor.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using ArdaManager.Application.Features.Docs.PurchaseOrders.Queries;
using ArdaManager.Client.Extensions;
using ArdaManager.Application.Features.Docs.PurchaseOrders.Commands;
using ArdaManager.Client.Infrastructure.Managers.GlobalVariable;
using ArdaManager.Application.Models.Chat;
using ArdaManager.Application.Responses.Identity;
using AutoMapper;
using Azure;
using ArdaManager.Domain.Entities.Transactions.Purchase;

namespace ArdaManager.Client.Pages.Docs.PurchaseOrder
{
    public partial class PurchaseOrders
    {
        [Inject] private IPurchaseOrderManager PurchaseOrderManager { get; set; }
        [Inject] private IGlobalVariableService GlobalVariableService { get; set; }
        [Inject] private IMapper _mapper { get; set; }

        [CascadingParameter] private HubConnection HubConnection { get; set; }
        //private IMapper _mapper;

        private List<PurchaseOrderResponse> _purchaseOrderList = new();
        private List<UserResponse> _userList = new();
        private PurchaseOrderResponse _purchaseOrder = new();
        private string _searchString = "";
        //private IMapper _mapper;

        private ClaimsPrincipal _currentUser;
        private bool _canCreatePurchaseOrders;
        private bool _canEditPurchaseOrders;
        private bool _canDeletePurchaseOrders;
        private bool _canSearchPurchaseOrders;
        private bool _loaded;

        protected override async Task OnInitializedAsync()
        {
            _currentUser = await _authenticationManager.CurrentUser();
            _canCreatePurchaseOrders = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.PurchaseOrders.Create)).Succeeded;
            _canEditPurchaseOrders = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.PurchaseOrders.Edit)).Succeeded;
            _canDeletePurchaseOrders = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.PurchaseOrders.Delete)).Succeeded;
            _canSearchPurchaseOrders = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.PurchaseOrders.Search)).Succeeded;

            await GetPurchaseOrdersAsync();
            _loaded = true;
            _userList = GlobalVariableService.AppUsers;
            HubConnection = HubConnection.TryInitialize(_navigationManager);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }
        }

        private async Task GetPurchaseOrdersAsync()
        {
            var response = await PurchaseOrderManager.GetAllAsync();
            if (response.Succeeded)
            {
                _purchaseOrderList = response.Data.ToList();
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
            string deleteContent = "Sil";
            var parameters = new DialogParameters
            {
                {nameof(Shared.Dialogs.DeleteConfirmation.ContentText), string.Format(deleteContent, id)}
            };
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<Shared.Dialogs.DeleteConfirmation>("Sil", parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                var response = await PurchaseOrderManager.DeleteAsync(id);
                if (response.Succeeded)
                {
                    await HubConnection.SendAsync(ApplicationConstants.SignalR.SendUpdateDashboard);
                    _snackBar.Add(response.Messages[0], Severity.Success);
                }
                else
                {
                    foreach (var message in response.Messages)
                    {
                        _snackBar.Add(message, Severity.Error);
                    }
                }
                await Reset();
            }
        }

        private async Task InvokeModal(int id = 0)
        {
            var parameters = new DialogParameters();
            if (id != 0)
            {
                _purchaseOrder = _purchaseOrderList.FirstOrDefault(c => c.Id == id);
                if (_purchaseOrder != null)
                {
                    try
                    {
                        UpsertPurchaseOrderCommand command = _mapper.Map<UpsertPurchaseOrderCommand>(_purchaseOrder);
                        parameters.Add(nameof(UpsertPurchaseOrderModal.UpsertPurchaseOrderModel), command);
                    }
                    catch (Exception ex)
                    {
                        var mes = ex.Message;
                        throw;
                    }

                }
            }
            else
            {
                int max_Id = 1;
                var max_Idrec = _purchaseOrderList.OrderByDescending(x => x.Id).FirstOrDefault();
                if (max_Idrec != null)
                    max_Id = max_Idrec.Id + 1;

                //string prefix = "KİE";
                string noStr = max_Id.ToString().PadLeft(7, '0');
                //string addedZeroToNoStr = noStr.PadLeft(7, '0');
                string newStr = "SAS" + noStr;
                parameters.Add(nameof(UpsertPurchaseOrderModal.UpsertPurchaseOrderModel), new UpsertPurchaseOrderCommand
                {
                    Id = 0,
                    DocDate = DateTime.UtcNow,
                    DocType = Domain.Enums.Doc.DocType.PurchaseOrder,
                    DocNo = newStr
                });
            }

            var options = new DialogOptionsEx
            {
                DragMode = MudDialogDragMode.WithoutBounds,
                Animations = new[] { AnimationType.Fade },
                Resizeable = true,
                MaxWidth = MaxWidth.ExtraLarge,
            };
            var dialog = await _dialogService.ShowEx<UpsertPurchaseOrderModal>(id == 0 ? "Yeni Teklif" : "Teklif Güncelle", parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                await Reset();
            }
        }

        private async Task Reset()
        {
            _purchaseOrder = new PurchaseOrderResponse();
            await GetPurchaseOrdersAsync();
        }

        private bool Search(PurchaseOrderResponse purchaseOrder)
        {
            if (string.IsNullOrWhiteSpace(_searchString)) return true;
            if (purchaseOrder.DocNo?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (purchaseOrder.Description?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            return false;
        }
        private Func<PurchaseOrderResponse, bool> _quickFilter => x =>
        {
            if (string.IsNullOrWhiteSpace(_searchString))
                return true;

            if (x.DocNo.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;

            if (x.Description.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;

            return false;
        };
    }
}