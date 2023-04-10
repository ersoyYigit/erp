using ArdaManager.Application.Features.CompanyFairs.Commands.AddEdit;
using ArdaManager.Application.Features.CompanyFairs.Queries.GetAll;
using ArdaManager.Application.Features.CompanyFairs.Queries.GetByCompany;
using ArdaManager.Application.Features.Fairs.Queries.GetAll;
using ArdaManager.Client.Infrastructure.Managers.Corporation.CompanyFairs;
using ArdaManager.Client.Infrastructure.Managers.Corporation.Fair;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Radzen.Blazor;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ArdaManager.Client.Pages.Corporation
{
    public partial class CompanyFairsTable
    {
        [Inject] private ICompanyFairManager companyFairManager { get; set; }
        [Inject] private IFairManager fairManager { get; set; }


        private MudTable<GetCompanyFairsByCompanyIdResponse> _table;
        private RadzenDropDownDataGrid<string> ddlFairs;
        [Parameter] public int CompanyId { get; set; }


        private List<GetAllFairsResponse> _fairs { get; set; }

        private List<GetCompanyFairsByCompanyIdResponse> _companyFairs = new();



        protected override async Task OnInitializedAsync()
        {
            await LoadDataAsync();
            await LoadAllDataAsync();


        }

        private async Task LoadDataAsync()
        {
            GetCompanyFairsByCompanyIdQuery query = new GetCompanyFairsByCompanyIdQuery() { CompanyId = CompanyId};
            var data = await companyFairManager.GetCompanyFairsByCompanyIdAsync(query);
            if (data.Succeeded)
            {
                _companyFairs = data.Data;
            }
        }

        private async Task LoadAllDataAsync()
        {
            GetAllCompanyFairsQuery query = new ();
            var data = await fairManager.GetAllAsync();
            if (data.Succeeded)
            {
                _fairs = data.Data;
            }
        }


        private async void OnChange(object value, string name)
        {
            var str = value is IEnumerable<object> ? string.Join(", ", (IEnumerable<object>)value) : value;

            Console.WriteLine($"{name} value changed to {str}");

            int selected = 0;
            if (value != null && int.TryParse(value.ToString(), out selected) && selected > 0)
            {
                AddEditCompanyFairCommand command = new AddEditCompanyFairCommand() { FairId = (int)value, CompanyId = CompanyId };
                var response = await companyFairManager.SaveAsync(command);
                if (response.Succeeded)
                {
                    _snackBar.Add(response.Messages[0], Severity.Success);
                    _fairs.Remove(_fairs.Find(x => x.Id == selected));
                    
                }
                else
                {
                    foreach (var message in response.Messages)
                    {
                        _snackBar.Add(message, Severity.Error);
                    }
                }
                ddlFairs.Reset();
                Reload();
            }


            
        }
        private async void Delete(int id)
        {
            var response = await companyFairManager.DeleteAsync(id);
            if (response.Succeeded)
            {
                _snackBar.Add(response.Messages[0], Severity.Success);
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
            Reload();
        }

        private async Task<TableData<GetCompanyFairsByCompanyIdResponse>> ServerReload(TableState state)
        {
            await LoadDataAsync();
            return new TableData<GetCompanyFairsByCompanyIdResponse> { TotalItems = _companyFairs.Count, Items = _companyFairs };
        }

        private void Reload()
        {
            _table.ReloadServerData();
        }
    }


}
