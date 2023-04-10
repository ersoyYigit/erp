using ArdaManager.Application.Features.Docs.PurchaseOffers.Queries;
using ArdaManager.Client.Infrastructure.Managers.Docs.PurchaseOffer;
using ArdaManager.Client.Shared.Components;
using ArdaManager.Shared.Constants.Permission;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArdaManager.Client.Shared.Dialogs.RelationPickers.Purchase
{
    public partial class PurchaseOfferPicker
    {
        [Inject] private IPurchaseOfferManager PurchaseOfferManager { get; set; }
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }

        [Parameter] public string SearchTerm { get; set; }

        private List<PurchaseOfferResponse> _purchaseOfferList = new();
        private PurchaseOfferResponse _purchaseOffer = new();
        private string _searchString = "";
        private bool _loaded;
        private bool _isRowSelected;

        protected override async Task OnInitializedAsync()
        {
            _searchString = SearchTerm;
            await GetPurchaseOffersAsync();
            _loaded = true;
        }

        private async Task GetPurchaseOffersAsync()
        {
            var response = await PurchaseOfferManager.GetAllAsync();
            if (response.Succeeded)
            {
                _purchaseOfferList = response.Data.Where(x => x.DocState == Domain.Enums.Doc.DocState.Approved).ToList();
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }



        private void SelectItem()
        {
            SelectProduct(_purchaseOffer);
        }

        private void OnSelectedItemsChanged(IEnumerable<PurchaseOfferResponse> items)
        {
            // İstediğiniz eylemleri burada gerçekleştirebilirsiniz.
            // Örneğin, seçilen öğelerin sayısını yazdırabilirsiniz:
            var item = items.FirstOrDefault();
            if (item != null )
            {
                _isRowSelected = true;
                _purchaseOffer = item;
                Console.WriteLine($"Selected Item: {item.Id}");
                Console.WriteLine($"Selected _purchaseOffer: {_purchaseOffer}");
            }
            else
                _isRowSelected = false;

            StateHasChanged();

        }

        private void SelectProduct(PurchaseOfferResponse offer)
        {
            //OnProductSelected.InvokeAsync(product);
            MudDialog.Close(DialogResult.Ok(offer));
            MudDialog.Close();
        }



        public void Cancel()
        {
            MudDialog.Cancel();
        }


        private async Task Reset()
        {
            _purchaseOffer = new PurchaseOfferResponse();
            await GetPurchaseOffersAsync();
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

    }
}
