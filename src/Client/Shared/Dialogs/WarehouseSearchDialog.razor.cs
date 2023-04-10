using ArdaManager.Application.Features.Products.Queries.Search;
using ArdaManager.Client.Infrastructure.Managers.Catalog.Product;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Collections.Generic;
using System.Threading.Tasks;
using ArdaManager.Client.Infrastructure.Managers.Inventory.Warehouse;
using ArdaManager.Application.Features.Warehouses.Queries.GetAll;

namespace ArdaManager.Client.Shared.Dialogs
{
    public partial class WarehouseSearchDialog
    {
        [Inject] private IWarehouseManager WarehouseManager { get; set; }
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }
        //[Parameter] public EventCallback<ProductSearchResultDto> OnProductSelected { get; set; }
        [Parameter] public string SearchTerm { get; set; }


        private List<GetAllWarehousesResponse> _warehouseList = new();
        private GetAllWarehousesResponse _warehouse = new();

        private MudTable<GetAllWarehousesResponse> Table;
        private MudTextField<string> SearchInputRef;


        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await SearchAsync(SearchTerm);
                //await Table.SetFocusAsync();
            }
        }
        private async Task SearchAsync(string term)
        {
            //SearchTerm = SearchInputRef.Value;
            var response = await WarehouseManager.GetAllAsync();
            if (response.Succeeded)
            {
                _warehouseList = response.Data;
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
            StateHasChanged();
        }



        private void SelectItem(TableRowClickEventArgs<GetAllWarehousesResponse> eventProduct)
        {
            SelectProduct((GetAllWarehousesResponse)eventProduct.Item);
        }

        private void SelectProduct(GetAllWarehousesResponse product)
        {
            //OnProductSelected.InvokeAsync(product);
            MudDialog.Close(DialogResult.Ok(product));
            MudDialog.Close();
        }



        void HandleIntervalElapsed(string debouncedText)
        {
            SearchAsync(debouncedText);
        }


        private async Task OnKeyDownAsync(KeyboardEventArgs e)
        {
            if (e.Key == "Enter" || e.Key == "F3")
            {
                StateHasChanged();
                //await SearchAsync();
            }
        }

        public void Cancel()
        {
            MudDialog.Cancel();
        }
    }
}
