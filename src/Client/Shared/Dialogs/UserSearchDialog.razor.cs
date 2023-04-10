using ArdaManager.Application.Features.Products.Queries.Search;
using ArdaManager.Application.Responses.Identity;
using ArdaManager.Client.Infrastructure.Managers.Catalog.Product;
using ArdaManager.Client.Pages.Docs.TemplateWorkOrder;
using Azure;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArdaManager.Client.Shared.Dialogs
{
    public partial class UserSearchDialog
    {
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }
        //[Parameter] public EventCallback<ProductSearchResultDto> OnProductSelected { get; set; }
        [Parameter] public string SearchTerm { get; set; }

        private List<UserResponse> _userList = new();
        private List<UserResponse> _allUserList = new();
        private MudTable<UserResponse> Table;
        private MudTextField<string> SearchInputRef;




        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await LoadData();
                SearchAsync(SearchTerm);
                //await Table.SetFocusAsync();
            }
        }




        async void HandleIntervalElapsed(string debouncedText)
        {
            SearchAsync(debouncedText);
        }

        private void SearchAsync(string term)
        {
            _userList = _allUserList.Where(x => string.IsNullOrEmpty(term) || x.FullName.Contains(term, StringComparison.OrdinalIgnoreCase)).ToList();
            StateHasChanged();
        }


        private async Task LoadData()
        {
            //SearchTerm = SearchInputRef.Value;
            var response = await _userManager.GetAllAsync();
            if (response.Succeeded)
            {
                _allUserList = response.Data.ToList();
                _userList = _allUserList;
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
        

        private void SelectItem(TableRowClickEventArgs<UserResponse> eventProduct)
        {
            SelectUser((UserResponse)eventProduct.Item);
        }

        private void SelectUser(UserResponse user)
        {
            //OnProductSelected.InvokeAsync(product);
            MudDialog.Close(DialogResult.Ok(user));
            MudDialog.Close();
        }

        public void Cancel()
        {
            MudDialog.Cancel();
        }

    }
}
