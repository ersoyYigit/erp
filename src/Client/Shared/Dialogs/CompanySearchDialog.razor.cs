
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Collections.Generic;
using System.Threading.Tasks;
using ArdaManager.Client.Infrastructure.Managers.Corporation.Company;
using ArdaManager.Application.Features.Companies.Queries.Search;

namespace ArdaManager.Client.Shared.Dialogs
{
    public partial class CompanySearchDialog
    {
        [Inject] private ICompanyManager CompanyManager { get; set; }

        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }
        //[Parameter] public EventCallback<CompanySearchResultDto> OnCompanySelected { get; set; }
        [Parameter] public string SearchTerm { get; set; }
        [Parameter] public CompanySearchQuery SearchQuery { get; set; }

        private List<CompanySearchResultDto> FilteredCompanys { get; set; } = new List<CompanySearchResultDto>();
        private MudTable<CompanySearchResultDto> Table;
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
            SearchQuery.SearchTerm = term;
            var response = await CompanyManager.GetFilteredAsync(SearchQuery);
            if (response.Succeeded)
            {
                FilteredCompanys = response.Data;
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



        private void SelectItem(TableRowClickEventArgs<CompanySearchResultDto> eventCompany)
        {
            SelectCompany((CompanySearchResultDto)eventCompany.Item);
        }

        private void SelectCompany(CompanySearchResultDto company)
        {
            //OnCompanySelected.InvokeAsync(company);
            MudDialog.Close(DialogResult.Ok(company));
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
