using ArdaManager.Application.Features.Docs.WarehouseRequests.Queries;
using ArdaManager.Client.Infrastructure.Managers.Docs.PurcheaseRequest;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using ArdaManager.Client.Infrastructure.Managers.Docs.WarehouseRequest;
using System.Linq;

namespace ArdaManager.Client.Shared.Dialogs.RelationPickers.Warehouse
{
    public partial class WarehouseRequestPicker
    {
        [Inject] private IWarehouseRequestManager WarehouseRequestManager { get; set; }
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }

        [Parameter] public string SearchTerm { get; set; }

        private List<WarehouseRequestResponse> _warehouseRequestList = new();
        private WarehouseRequestResponse _warehouseRequest = new();
        private string _searchString = "";
        private bool _loaded;
        private bool _isRowSelected;

        protected override async Task OnInitializedAsync()
        {
            _searchString = SearchTerm;
            await GetWarehouseRequestsAsync();
            _loaded = true;
        }

        private async Task GetWarehouseRequestsAsync()
        {
            var response = await WarehouseRequestManager.GetAllAsync();
            if (response.Succeeded)
            {
                _warehouseRequestList = response.Data.Where(x=>x.DocState != Domain.Enums.Doc.DocState.Finished).ToList();
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
            SelectProduct(_warehouseRequest);
        }

        private void OnSelectedItemsChanged(IEnumerable<WarehouseRequestResponse> items)
        {
            // İstediğiniz eylemleri burada gerçekleştirebilirsiniz.
            // Örneğin, seçilen öğelerin sayısını yazdırabilirsiniz:
            var item = items.LastOrDefault();
            if (item != null /*&& item.DocState == Domain.Enums.Doc.DocState.Approved*/)
            {
                _isRowSelected = true;
                _warehouseRequest = item;
                Console.WriteLine($"Selected Item: {item.Id}");
                Console.WriteLine($"Selected _warehouseRequest: {_warehouseRequest}");
            }
            else
                _isRowSelected = false;

            StateHasChanged();

        }



        private void OnSelectedItemChangedTable(WarehouseRequestResponse item)
        {

            // İstediğiniz eylemleri burada gerçekleştirebilirsiniz.
            // Örneğin, seçilen öğelerin sayısını yazdırabilirsiniz:
            //var item = items.LastOrDefault();
            if (item != null /*&& item.DocState == Domain.Enums.Doc.DocState.Approved*/)
            {
                _isRowSelected = true;
                _warehouseRequest = item;
                Console.WriteLine($"Selected Item: {item.Id}");
                Console.WriteLine($"Selected _warehouseRequest: {_warehouseRequest}");
            }
            else
                _isRowSelected = false;

            StateHasChanged();


        }

        private void SelectProduct(WarehouseRequestResponse request)
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
            _warehouseRequest = new WarehouseRequestResponse();
            await GetWarehouseRequestsAsync();
        }
        private Func<WarehouseRequestResponse, bool> _quickFilter => x =>
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
