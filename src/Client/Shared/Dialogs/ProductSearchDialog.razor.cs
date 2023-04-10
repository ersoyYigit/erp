using ArdaManager.Application.Features.Products.Queries.Search;
using ArdaManager.Client.Infrastructure.Managers.Catalog.Product;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Collections.Generic;
using System.Threading.Tasks;
using ArdaManager.Domain.Entities.Transactions.Purchase;
using ArdaManager.Domain.Entities.Catalog;
using Azure;
using ArdaManager.Client.Infrastructure.Managers.Catalog.Pattern;
using MudBlazor.Extensions.Components;
using static MudBlazor.CategoryTypes;
using ArdaManager.Client.Pages.Docs.PurchaseRequest;
using Microsoft.AspNetCore.SignalR.Client;
using System;

namespace ArdaManager.Client.Shared.Dialogs
{

    public partial class ProductSearchDialog
    {
        [Inject] private IProductManager ProductManager { get; set; }

        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }
        //[Parameter] public EventCallback<ProductSearchResultDto> OnProductSelected { get; set; }
        [Parameter] public string SearchTerm { get; set; }

        
        private List<ProductSearchResultDto> FilteredProducts { get; set; } = new List<ProductSearchResultDto>();
        private MudTable<ProductSearchResultDto> Table;
        private MudTextField<string> SearchInputRef;


        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender) 
            {
                await SearchAsync(SearchTerm);
                //await Table.SetFocusAsync();
            }
        }
        protected override async Task OnInitializedAsync()
        {

        }
        private async Task SearchAsync(string term)
        {
            //SearchTerm = SearchInputRef.Value;
            var response = await ProductManager.GetFilteredAsync(term);
            if (response.Succeeded)
            {
                FilteredProducts = response.Data;
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

        

        private void SelectItem(TableRowClickEventArgs<ProductSearchResultDto> eventProduct)
        {
            SelectProduct((ProductSearchResultDto)eventProduct.Item);
        }

        private void SelectProduct(ProductSearchResultDto product)
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


        private Func<ProductSearchResultDto, string> _cellStyleFunc => x =>
        {
            string style = "";

            if (x.Quantity <= 0)
                style += "background-color:#FD4A87";
            else
                style += "background-color:#8CED8C";

            return style;
        };

    }
}
