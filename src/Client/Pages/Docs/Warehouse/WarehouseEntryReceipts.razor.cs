using ArdaManager.Application.Features.Docs.WarehouseReceipts.Commands.AddEdit;
using ArdaManager.Application.Features.Docs.WarehouseReceipts.Queries.GetAll;
using ArdaManager.Client.Infrastructure.Managers.Docs.WarehouseReceipt;
using ArdaManager.Domain.Enums.Doc;
using ArdaManager.Shared.Constants.Application;
using ArdaManager.Shared.Constants.Permission;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor.Extensions.Options;
using MudBlazor.Extensions;
using MudBlazor;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Claims;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Authorization;
using ArdaManager.Client.Extensions;
using System.Linq;
using ArdaManager.Client.Infrastructure.Managers.GlobalVariable;
using AutoMapper;
using ArdaManager.Application.Responses.Identity;

namespace ArdaManager.Client.Pages.Docs.Warehouse
{
    public partial class WarehouseEntryReceipts
    {
        [Inject] private IWarehouseReceiptManager WarehouseReceiptManager { get; set; }
        [Inject] private IGlobalVariableService GlobalVariableService { get; set; }
        [Inject] private IMapper _mapper { get; set; }

        [Parameter] public int? Id { get; set; }
        [Parameter] public string docno { get; set; }
        [CascadingParameter] private HubConnection HubConnection { get; set; }
        //private IMapper _mapper;

        private List<GetAllWarehouseReceiptsResponse> _warehouseReceiptList = new();
        private List<UserResponse> _userList = new();
        private GetAllWarehouseReceiptsResponse _warehouseReceipt = new();
        private string _searchString = "";
        //private IMapper _mapper;

        private ClaimsPrincipal _currentUser;
        private bool _canCreateWarehouseReceipts;
        private bool _canEditWarehouseReceipts;
        private bool _canDeleteWarehouseReceipts;
        private bool _canSearchWarehouseReceipts;
        private bool _loaded;

        protected override async Task OnInitializedAsync()
        {
            _currentUser = await _authenticationManager.CurrentUser();
            _canCreateWarehouseReceipts = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.WarehouseReceipts.Create)).Succeeded;
            _canEditWarehouseReceipts = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.WarehouseReceipts.Edit)).Succeeded;
            _canDeleteWarehouseReceipts = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.WarehouseReceipts.Delete)).Succeeded;
            _canSearchWarehouseReceipts = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.WarehouseReceipts.Search)).Succeeded;

            await GetWarehouseReceiptsAsync();
            _loaded = true;
            _userList = GlobalVariableService.AppUsers;
            HubConnection = HubConnection.TryInitialize(_navigationManager);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }
        }

        private async Task GetWarehouseReceiptsAsync()
        {
            var response = await WarehouseReceiptManager.GetAllByType(WarehouseReceiptType.Entrance);
            if (response.Succeeded)
            {
                _warehouseReceiptList = response.Data.ToList();
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
                var response = await WarehouseReceiptManager.DeleteAsync(id);
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
                _warehouseReceipt = _warehouseReceiptList.FirstOrDefault(c => c.Id == id);
                if (_warehouseReceipt != null)
                {
                    try
                    {
                        AddEditWarehouseReceiptCommand command = _mapper.Map<AddEditWarehouseReceiptCommand>(_warehouseReceipt);
                        parameters.Add(nameof(AddEditWarehouseEntryReceiptModal.AddEditWarehouseReceiptModel), command);
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
                var max_Idrec = _warehouseReceiptList.OrderByDescending(x => x.Id).FirstOrDefault();
                if (max_Idrec != null)
                    max_Id = max_Idrec.Id + 1;

                //string prefix = "KİE";
                string noStr = max_Id.ToString().PadLeft(7, '0');
                //string addedZeroToNoStr = noStr.PadLeft(7, '0');
                string newStr = "DGF" + noStr;
                parameters.Add(nameof(AddEditWarehouseEntryReceiptModal.AddEditWarehouseReceiptModel), new AddEditWarehouseReceiptCommand
                {
                    Id = 0,
                    DocDate = DateTime.UtcNow,
                    DocType = Domain.Enums.Doc.DocType.WarehouseEntrance,
                    WarehouseReceiptType = WarehouseReceiptType.Entrance,
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
            var dialog = await _dialogService.ShowEx<AddEditWarehouseEntryReceiptModal>(id == 0 ? "Yeni Depo Fişi" : "Depo Fişi Güncelle", parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                await Reset();
            }
        }

        private async Task Reset()
        {
            _warehouseReceipt = new GetAllWarehouseReceiptsResponse();
            await GetWarehouseReceiptsAsync();
        }

        private bool Search(GetAllWarehouseReceiptsResponse warehouseReceipt)
        {
            if (string.IsNullOrWhiteSpace(_searchString)) return true;
            if (warehouseReceipt.DocNo?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (warehouseReceipt.Description?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            return false;
        }
        private Func<GetAllWarehouseReceiptsResponse, bool> _quickFilter => x =>
        {
            if (string.IsNullOrWhiteSpace(_searchString))
                return true;

            if (x.DocNo.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;

            if (x.Description.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;

            if (x.RelatedCompanyName.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;

            return false;
        };
    }
}