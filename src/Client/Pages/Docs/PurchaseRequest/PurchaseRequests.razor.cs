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
using ArdaManager.Client.Infrastructure.Managers.Docs.PurcheaseRequest;
using ArdaManager.Application.Features.Docs.PurchaseRequests.Queries;
using ArdaManager.Client.Pages.Docs.Warehouse;
using ArdaManager.Application.Features.Docs.PurchaseRequests.Commands.AddEdit;
using ArdaManager.Domain.Entities.Transactions.Purchase;

namespace ArdaManager.Client.Pages.Docs.PurchaseRequest
{
    public partial class PurchaseRequests
    {
        [Inject] private IPurchaseRequestManager PurchaseRequestManager { get; set; }

        [CascadingParameter] private HubConnection HubConnection { get; set; }
        
        [Parameter] public int? Id { get; set; }

        private List<PurchaseRequestDto> _purchaseRequestList = new();
        private PurchaseRequestDto _purchaseRequest = new();
        private string _searchString = "";


        private ClaimsPrincipal _currentUser;
        private bool _canCreatePurchaseRequests;
        private bool _canEditPurchaseRequests;
        private bool _canDeletePurchaseRequests;
        private bool _canSearchPurchaseRequests;
        private bool _loaded;

        protected override async Task OnInitializedAsync()
        {
            _currentUser = await _authenticationManager.CurrentUser();
            _canCreatePurchaseRequests = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.PurchaseRequests.Create)).Succeeded;
            _canEditPurchaseRequests = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.PurchaseRequests.Edit)).Succeeded;
            _canDeletePurchaseRequests = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.PurchaseRequests.Delete)).Succeeded;
            _canSearchPurchaseRequests = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.PurchaseRequests.Search)).Succeeded;

            await GetPurchaseRequestsAsync();
            _loaded = true;


            if (Id.HasValue)
            {
                await InvokeModal((int)Id);
            }
            HubConnection = HubConnection.TryInitialize(_navigationManager);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }
        }

        private async Task GetPurchaseRequestsAsync()
        {
            var response = await PurchaseRequestManager.GetAllAsync();
            if (response.Succeeded)
            {
                _purchaseRequestList = response.Data.ToList();
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
                var response = await PurchaseRequestManager.DeleteAsync(id);
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
                _purchaseRequest = _purchaseRequestList.FirstOrDefault(c => c.Id == id);
                if (_purchaseRequest != null)
                {
                    parameters.Add(nameof(AddEditPurchaseRequestModal.AddEditPurchaseRequestModel), new AddEditPurchaseRequestCommand
                    {
                        Id = _purchaseRequest.Id,
                        Description = _purchaseRequest.Description,
                        DocDate= _purchaseRequest.DocDate,
                        DocState = _purchaseRequest.DocState,
                        DocNo  = _purchaseRequest.DocNo,
                        DocType = DocType.PurchaseRequest,
                        RequesterId = _purchaseRequest.RequesterId,
                        RequesterName = _purchaseRequest.RequesterName,
                        RequesterDepartment = _purchaseRequest.RequesterDepartment,
                        
                        PurchaseRequestRowsDto = _purchaseRequest.PurchaseRequestRowsDto
                    });
                }
            }
            else
            {
                int max_Id = 1;
                var max_Idrec = _purchaseRequestList.OrderByDescending(x => x.Id).FirstOrDefault();
                if (max_Idrec != null)
                    max_Id = max_Idrec.Id + 1;

                //string prefix = "KİE";
                string noStr = max_Id.ToString().PadLeft(7, '0');
                //string addedZeroToNoStr = noStr.PadLeft(7, '0');
                string newStr = "STF" + noStr;

                parameters.Add(nameof(AddEditPurchaseRequestModal.AddEditPurchaseRequestModel), new AddEditPurchaseRequestCommand
                {
                    DocDate = DateTime.UtcNow,
                    DocType = DocType.PurchaseRequest,
                    DocNo = newStr,
                    PurchaseRequestRowsDto = new List<PurchaseRequestRowDto>()

                }); 
            }

            var options = new DialogOptionsEx
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
            var dialog = await _dialogService.ShowEx<AddEditPurchaseRequestModal>(id == 0 ? _localizer["Create"] : _localizer["Edit"], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                await Reset();
            }
        }

        private async Task Reset()
        {
            _purchaseRequest = new PurchaseRequestDto();
            await GetPurchaseRequestsAsync();
        }

        private bool Search(PurchaseRequestDto purchaseRequest)
        {
            if (string.IsNullOrWhiteSpace(_searchString)) return true;
            if (purchaseRequest.DocNo?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (purchaseRequest.Description?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            return false;
        }
        private Func<PurchaseRequestDto, bool> _quickFilter => x =>
        {
            if (string.IsNullOrWhiteSpace(_searchString))
                return true;

            if (x.DocNo.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;

            if (x.Description.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            
            if (x.RequesterName.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            
            

            return false;
        };

    }
}
