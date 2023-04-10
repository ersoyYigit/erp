using ArdaManager.Client.Infrastructure.Managers.Docs.PurchaseOffer;
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
using ArdaManager.Application.Features.Docs.PurchaseOffers.Queries;
using ArdaManager.Client.Extensions;
using ArdaManager.Application.Features.Docs.PurchaseOffers.Commands;
using ArdaManager.Client.Infrastructure.Managers.GlobalVariable;
using ArdaManager.Application.Models.Chat;
using ArdaManager.Application.Responses.Identity;
using AutoMapper;
using Azure;
using ArdaManager.Domain.Entities.Transactions.Purchase;
using static MudBlazor.CategoryTypes;

namespace ArdaManager.Client.Pages.Docs.PurchaseOffer
{
    public partial class PurchaseOffers
    {
        [Inject] private IPurchaseOfferManager PurchaseOfferManager { get; set; }
        [Inject] private IGlobalVariableService GlobalVariableService { get; set; }
        [Inject] private IMapper _mapper { get; set; }

        [CascadingParameter] private HubConnection HubConnection { get; set; }
        //private IMapper _mapper;

        private List<PurchaseOfferResponse> _purchaseOfferList = new();
        private List<UserResponse> _userList = new();
        private PurchaseOfferResponse _purchaseOffer = new();
        private string _searchString = "";
        //private IMapper _mapper;

        private ClaimsPrincipal _currentUser;
        private bool _canCreatePurchaseOffers;
        private bool _canEditPurchaseOffers;
        private bool _canDeletePurchaseOffers;
        private bool _canSearchPurchaseOffers;
        private bool _loaded;

        protected override async Task OnInitializedAsync()
        {
            _currentUser = await _authenticationManager.CurrentUser();
            _canCreatePurchaseOffers = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.PurchaseOffers.Create)).Succeeded;
            _canEditPurchaseOffers = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.PurchaseOffers.Edit)).Succeeded;
            _canDeletePurchaseOffers = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.PurchaseOffers.Delete)).Succeeded;
            _canSearchPurchaseOffers = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.PurchaseOffers.Search)).Succeeded;

            await GetPurchaseOffersAsync();
            _loaded = true;
            _userList = GlobalVariableService.AppUsers;
            HubConnection = HubConnection.TryInitialize(_navigationManager);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }
        }

        private async Task GetPurchaseOffersAsync()
        {
            var response = await PurchaseOfferManager.GetAllAsync();
            if (response.Succeeded)
            {
                _purchaseOfferList = response.Data.ToList();
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
                var response = await PurchaseOfferManager.DeleteAsync(id);
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

        private async Task ChooseOffer(int id)
        {
            ChoosePurchaseOfferCommand command = new ChoosePurchaseOfferCommand() { Id = id };
            var response = await PurchaseOfferManager.ChooseAsync(command);
            if (response.Succeeded)
            {
                _snackBar.Add(response.Messages[0], Severity.Success);
                await Reset();
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }
        private async Task InvokeModal(int id = 0)
        {
            var parameters = new DialogParameters();
            if (id != 0)
            {
                _purchaseOffer = _purchaseOfferList.FirstOrDefault(c => c.Id == id);
                if (_purchaseOffer != null)
                {
                    try
                    {
                        UpsertPurchaseOfferCommand command = _mapper.Map<UpsertPurchaseOfferCommand>(_purchaseOffer);
                        parameters.Add(nameof(UpsertPurchaseOfferModal.UpsertPurchaseOfferModel), command);
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
                var max_Idrec = _purchaseOfferList.OrderByDescending(x => x.Id).FirstOrDefault();
                if (max_Idrec != null)
                    max_Id = max_Idrec.Id + 1;

                //string prefix = "KİE";
                string noStr = max_Id.ToString().PadLeft(7, '0');
                //string addedZeroToNoStr = noStr.PadLeft(7, '0');
                string newStr = "SAT" + noStr;
                parameters.Add(nameof(UpsertPurchaseOfferModal.UpsertPurchaseOfferModel), new UpsertPurchaseOfferCommand
                {
                    Id = 0,
                    DocDate = DateTime.UtcNow,
                    DocType = Domain.Enums.Doc.DocType.PurchaseOffer,
                    DocNo = newStr
                });
            }
            
            var options = new DialogOptionsEx {
                DragMode = MudDialogDragMode.WithoutBounds,
                Animations = new[] { AnimationType.Fade },
                Resizeable = true,
                MaxWidth = MaxWidth.ExtraLarge,
            };
            var dialog = await _dialogService.ShowEx<UpsertPurchaseOfferModal>(id == 0 ? "Yeni Teklif" : "Teklif Güncelle", parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                await Reset();
            }
        }

        private async Task Reset()
        {
            _purchaseOffer = new PurchaseOfferResponse();
            await GetPurchaseOffersAsync();
        }

        private bool Search(PurchaseOfferResponse purchaseOffer)
        {
            if (string.IsNullOrWhiteSpace(_searchString)) return true;
            if (purchaseOffer.DocNo?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (purchaseOffer.Description?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            return false;
        }
        private Func<PurchaseOfferResponse, bool> _quickFilter => x =>
        {
            if (string.IsNullOrWhiteSpace(_searchString))
                return true;

            if (x.DocNo.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;

            if (x.Description.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;

            if (x.CompanyName.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;

            return false;
        };


        Func<PurchaseOfferResponse, object> _groupBy = x =>
        {
            return x.PrevDoc.DocNo;
        };
    }
}