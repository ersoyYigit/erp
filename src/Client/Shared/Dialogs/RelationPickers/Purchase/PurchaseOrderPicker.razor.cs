using ArdaManager.Application.Features.Docs.PurchaseOrders.Queries;
using ArdaManager.Client.Infrastructure.Managers.Docs.PurchaseOrder;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArdaManager.Client.Shared.Dialogs.RelationPickers.Purchase
{
    public partial class PurchaseOrderPicker
    {
        [Inject] private IPurchaseOrderManager PurchaseOrderManager { get; set; }
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }

        [Parameter] public string SearchTerm { get; set; }

        private List<PurchaseOrderResponse> _purchaseOrderList = new();
        private PurchaseOrderResponse _purchaseOrder = new();
        private string _searchString = "";
        private bool _loaded;
        private bool _isRowSelected;

        protected override async Task OnInitializedAsync()
        {
            _searchString = SearchTerm;
            await GetPurchaseOrdersAsync();
            _loaded = true;
        }

        private async Task GetPurchaseOrdersAsync()
        {
            var response = await PurchaseOrderManager.GetAllAsync();
            if (response.Succeeded)
            {
                _purchaseOrderList = response.Data/*.Where(x => x.DocState == Domain.Enums.Doc.DocState.Approved)*/.ToList();
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
            SelectProduct(_purchaseOrder);
        }

        private void OnSelectedItemsChanged(IEnumerable<PurchaseOrderResponse> items)
        {
            // İstediğiniz eylemleri burada gerçekleştirebilirsiniz.
            // Örneğin, seçilen öğelerin sayısını yazdırabilirsiniz:
            var item = items.FirstOrDefault();
            if (item != null)
            {
                _isRowSelected = true;
                _purchaseOrder = item;
            }
            else
                _isRowSelected = false;

            StateHasChanged();

        }

        private void SelectProduct(PurchaseOrderResponse offer)
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
            _purchaseOrder = new PurchaseOrderResponse();
            await GetPurchaseOrdersAsync();
        }
        private Func<PurchaseOrderResponse, bool> _quickFilter => x =>
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
