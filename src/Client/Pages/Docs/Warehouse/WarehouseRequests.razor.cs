using ArdaManager.Application.Features.Docs.WarehouseRequests.Commands;
using ArdaManager.Application.Features.Docs.WarehouseRequests.Queries;
using ArdaManager.Application.Responses.Identity;
using ArdaManager.Client.Extensions;
using ArdaManager.Client.Infrastructure.Managers.Docs.WarehouseRequest;
using ArdaManager.Client.Infrastructure.Managers.GlobalVariable;
using ArdaManager.Shared.Constants.Application;
using ArdaManager.Shared.Constants.Permission;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;
using MudBlazor.Extensions;
using MudBlazor.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ArdaManager.Client.Pages.Docs.Warehouse
{
    public partial class WarehouseRequests
    {
        [Inject] private IWarehouseRequestManager WarehouseRequestManager { get; set; }
        [Inject] private IGlobalVariableService GlobalVariableService { get; set; }
        [Inject] private IMapper _mapper { get; set; }

        [Parameter] public int? Id { get; set; }
        [Parameter] public string docno { get; set; }
        [CascadingParameter] private HubConnection HubConnection { get; set; }
        //private IMapper _mapper;

        private List<WarehouseRequestResponse> _warehouseRequestList = new();
        private List<UserResponse> _userList = new();
        private WarehouseRequestResponse _warehouseRequest = new();
        private string _searchString = "";
        //private IMapper _mapper;

        private ClaimsPrincipal _currentUser;
        private bool _canCreateWarehouseRequests;
        private bool _canEditWarehouseRequests;
        private bool _canDeleteWarehouseRequests;
        private bool _canSearchWarehouseRequests;
        private bool _loaded;

        protected override async Task OnInitializedAsync()
        {
            _currentUser = await _authenticationManager.CurrentUser();
            _canCreateWarehouseRequests = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.WarehouseRequests.Create)).Succeeded;
            _canEditWarehouseRequests = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.WarehouseRequests.Edit)).Succeeded;
            _canDeleteWarehouseRequests = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.WarehouseRequests.Delete)).Succeeded;

            await GetWarehouseRequestsAsync();
            _loaded = true;
            _userList = GlobalVariableService.AppUsers;
            HubConnection = HubConnection.TryInitialize(_navigationManager);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }
        }

        private async Task GetWarehouseRequestsAsync()
        {
            var response = await WarehouseRequestManager.GetAllAsync();
            if (response.Succeeded)
            {
                _warehouseRequestList = response.Data.ToList();
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
                var response = await WarehouseRequestManager.DeleteAsync(id);
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
                _warehouseRequest = _warehouseRequestList.FirstOrDefault(c => c.Id == id);
                if (_warehouseRequest != null)
                {
                    try
                    {
                        UpsertWarehouseRequestCommand command = _mapper.Map<UpsertWarehouseRequestCommand>(_warehouseRequest);
                        parameters.Add(nameof(UpsertWarehouseRequestModal.UpsertWarehouseRequestModel), command);
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
                var max_Idrec = _warehouseRequestList.OrderByDescending(x => x.Id).FirstOrDefault();
                if (max_Idrec != null)
                    max_Id = max_Idrec.Id + 1;

                //string prefix = "KİE";
                string noStr = max_Id.ToString().PadLeft(7, '0');
                //string addedZeroToNoStr = noStr.PadLeft(7, '0');
                string newStr = "DTT" + noStr;
                parameters.Add(nameof(UpsertWarehouseRequestModal.UpsertWarehouseRequestModel), new UpsertWarehouseRequestCommand
                {
                    Id = 0,
                    DocDate = DateTime.UtcNow,
                    DocType = Domain.Enums.Doc.DocType.WarehouseRequest,
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
            var dialog = await _dialogService.ShowEx<UpsertWarehouseRequestModal>(id == 0 ? "Yeni Depo Fişi" : "Depo Fişi Güncelle", parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                await Reset();
            }
        }

        private async Task Reset()
        {
            _warehouseRequest = new WarehouseRequestResponse();
            await GetWarehouseRequestsAsync();
        }

        private bool Search(WarehouseRequestResponse warehouseRequest)
        {
            if (string.IsNullOrWhiteSpace(_searchString)) return true;
            if (warehouseRequest.DocNo?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (warehouseRequest.Description?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            return false;
        }
        private Func<WarehouseRequestResponse, bool> _quickFilter => x =>
        {
            if (string.IsNullOrWhiteSpace(_searchString))
                return true;

            if (x.DocNo.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;

            if (x.Description.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;

            if (x.WarehouseName.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;

            if (x.RelatedWarehouseName.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;

            return false;
        };
    }
}
