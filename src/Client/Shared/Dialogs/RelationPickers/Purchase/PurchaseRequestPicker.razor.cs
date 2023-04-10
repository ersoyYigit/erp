using ArdaManager.Application.Features.Docs.PurchaseRequests.Queries;
using ArdaManager.Client.Infrastructure.Managers.Docs.PurcheaseRequest;
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
    public partial class PurchaseRequestPicker
    {
        [Inject] private IPurchaseRequestManager PurchaseRequestManager { get; set; }
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }

        [Parameter] public string SearchTerm { get; set; }

        private List<PurchaseRequestDto> _purchaseRequestList = new();
        private PurchaseRequestDto _purchaseRequest = new();
        private string _searchString = "";
        private bool _loaded;
        private bool _isRowSelected;

        protected override async Task OnInitializedAsync()
        {
            _searchString = SearchTerm;
            await GetPurchaseRequestsAsync();
            _loaded = true;
        }

        private async Task GetPurchaseRequestsAsync()
        {
            var response = await PurchaseRequestManager.GetAllAsync();
            if (response.Succeeded)
            {
                _purchaseRequestList = response.Data.Where(x => x.DocState == Domain.Enums.Doc.DocState.Approved).ToList();
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
            SelectProduct(_purchaseRequest);
        }

        private void OnSelectedItemsChanged(IEnumerable<PurchaseRequestDto> items)
        {
            // İstediğiniz eylemleri burada gerçekleştirebilirsiniz.
            // Örneğin, seçilen öğelerin sayısını yazdırabilirsiniz:
            var item = items.LastOrDefault();
            if (item != null && item.DocState == Domain.Enums.Doc.DocState.Approved)
            {
                _isRowSelected = true;
                _purchaseRequest = item;
                Console.WriteLine($"Selected Item: {item.Id}");
                Console.WriteLine($"Selected _purchaseRequest: {_purchaseRequest}");
            }
            else
                _isRowSelected = false;

            StateHasChanged();
            
        }



        private void OnSelectedItemChangedTable(PurchaseRequestDto item)
        {
            
            // İstediğiniz eylemleri burada gerçekleştirebilirsiniz.
            // Örneğin, seçilen öğelerin sayısını yazdırabilirsiniz:
            //var item = items.LastOrDefault();
            if (item != null && item.DocState == Domain.Enums.Doc.DocState.Approved)
            {
                _isRowSelected = true;
                _purchaseRequest = item;
                Console.WriteLine($"Selected Item: {item.Id}");
                Console.WriteLine($"Selected _purchaseRequest: {_purchaseRequest}");
            }
            else
                _isRowSelected = false;

            StateHasChanged();
            

        }

        private void SelectProduct(PurchaseRequestDto request)
        {
            //OnProductSelected.InvokeAsync(product);
            MudDialog.Close(DialogResult.Ok(request));
            MudDialog.Close();
        }



        public void Cancel()
        {
            MudDialog.Cancel();
        }


        private async Task Reset()
        {
            _purchaseRequest = new PurchaseRequestDto();
            await GetPurchaseRequestsAsync();
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
