using ArdaManager.Application.Features.Companies.Commands.AddEdit;
using ArdaManager.Application.Requests;
using ArdaManager.Client.Extensions;
using ArdaManager.Shared.Constants.Application;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Blazored.FluentValidation;
using ArdaManager.Client.Infrastructure.Managers.Corporation.Company;
using ArdaManager.Client.Infrastructure.Managers.Catalog;
using ArdaManager.Application.Features.Patterns.Queries.GetAll;
using ArdaManager.Client.Infrastructure.Managers.Addressing.City;
using ArdaManager.Client.Infrastructure.Managers.Addressing.Country;
using ArdaManager.Application.Features.Countries.Queries.GetAll;
using ArdaManager.Application.Features.Cities.Queries.GetAll;
using ArdaManager.Application.Features.Cities.Queries.GetCitiesWithCountryId;
using System.Xml.Linq;
using ArdaManager.Client.Infrastructure.Managers.Corporation.Fair;
using ArdaManager.Application.Features.Fairs.Queries.GetAll;

namespace ArdaManager.Client.Pages.Corporation
{
    
    public partial class AddEditCompanyModal
    {
        [Inject] private ICompanyManager CompanyManager { get; set; }
        [Inject] private ICityManager CityManager { get; set; }
        [Inject] private ICountryManager CountryManager { get; set; }
        [Inject] private IFairManager FairManager { get; set; }

        [Parameter] public AddEditCompanyCommand AddEditCompanyModel { get; set; } = new();
        [CascadingParameter] private HubConnection HubConnection { get; set; }
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }

        private FluentValidationValidator _fluentValidationValidator;
        private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });
        private List<GetAllCountriesResponse> _countries = new();
        //private List<GetAllCitiesResponse> _cities = new();
        private List<GetCitiesByCountryIdResponse> _cities = new();
        private List<GetAllFairsResponse> _fairs = new();
        IEnumerable<int> multipleValues = new int[] { };

        public void Cancel()
        {
            MudDialog.Cancel();
        }

        private async Task SaveAsync()
        {
            var response = await CompanyManager.SaveAsync(AddEditCompanyModel);
            if (response.Succeeded)
            {
                _snackBar.Add(response.Messages[0], Severity.Success);
                await HubConnection.SendAsync(ApplicationConstants.SignalR.SendUpdateDashboard);
                MudDialog.Close();
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }

        protected override async Task OnInitializedAsync()
        {
            await LoadDataAsync();
            HubConnection = HubConnection.TryInitialize(_navigationManager);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }
        }

        private async Task LoadDataAsync()
        {
            //await LoadImageAsync();
            await LoadCountries();
            await LoadCities();
            await LoadFairs();
        }

        private async Task LoadCountries()
        {
            var data = await CountryManager.GetAllAsync();
            if (data.Succeeded)
            {
                _countries = data.Data;
            }
        }

        private async Task LoadFairs()
        {
            //GetCitiesByCountryIdQuery query = new GetCitiesByCountryIdQuery() { CountryId = AddEditCompanyModel.CountryId };
            var data = await FairManager.GetAllAsync();
            //var data = await CityManager.GetAllAsync();
            if (data.Succeeded)
            {
                //_countryCities = data.Data;
                _fairs = data.Data;
            }

        }
        private async Task LoadCities()
        {
            if (AddEditCompanyModel.CountryId != 0)
            {
                GetCitiesByCountryIdQuery query = new GetCitiesByCountryIdQuery() { CountryId = AddEditCompanyModel.CountryId };
                var data = await CityManager.GetCitiesByCountryIdAsync(query);
                //var data = await CityManager.GetAllAsync();
                if (data.Succeeded)
                {
                    //_countryCities = data.Data;
                    _cities = data.Data;
                }
            }
            
        }
        
        void OnChange(object value, string name)
        {
            var str = value is IEnumerable<object> ? string.Join(", ", (IEnumerable<object>)value) : value;

            Console.WriteLine($"{name} value changed to {str}");
        }
        private async Task LoadCountryCities()
        {

            GetCitiesByCountryIdQuery query = new GetCitiesByCountryIdQuery() { CountryId = AddEditCompanyModel.CountryId };
            var data = await CityManager.GetCitiesByCountryIdAsync(query);
            //var data = await CityManager.GetAllAsync();
            if (data.Succeeded)
            {
                //_countryCities = data.Data;
                _cities = data.Data;
            }
        }

        /*
        private async Task LoadPatternsAsync()
        {
            var data = await PatternManager.GetAllAsync();
            if (data.Succeeded)
            {
                _patterns = data.Data;
            }
        }
        */

        /*
        private async Task LoadImageAsync()
        {
            var data = await CompanyManager.GetCompanyImageAsync(AddEditCompanyModel.Id);
            if (data.Succeeded)
            {
                var imageData = data.Data;
                if (!string.IsNullOrEmpty(imageData))
                {
                    AddEditCompanyModel.ImageDataURL = imageData;
                }
            }
        }
        */

        private void DeleteAsync()
        {
            //AddEditCompanyModel.ImageDataURL = null;
            //AddEditCompanyModel.UploadRequest = new UploadRequest();
        }

        //private IBrowserFile _file;
/*
        private async Task UploadFiles(InputFileChangeEventArgs e)
        {
            _file = e.File;
            if (_file != null)
            {
                var extension = Path.GetExtension(_file.Name);
                var format = "image/png";
                var imageFile = await e.File.RequestImageFileAsync(format, 400, 400);
                var buffer = new byte[imageFile.Size];
                await imageFile.OpenReadStream().ReadAsync(buffer);
                AddEditCompanyModel.ImageDataURL = $"data:{format};base64,{Convert.ToBase64String(buffer)}";
                AddEditCompanyModel.UploadRequest = new UploadRequest { Data = buffer, UploadType = Application.Enums.UploadType.Company, Extension = extension };
            }
        }
*/
        private async Task<IEnumerable<int>> SearchCountries(string value)
        {
            // In real life use an asynchronous function for fetching data from an api.
            await Task.Delay(5);

            // if text is null or empty, show complete list
            if (string.IsNullOrEmpty(value))
                return _countries.Select(x => x.Id);

            return _countries.Where(x => x.Name.Contains(value, StringComparison.InvariantCultureIgnoreCase))
                .Select(x => x.Id);
        }
        private async Task<IEnumerable<int>> SearchCities(string value)
        {
            
            // In real life use an asynchronous function for fetching data from an api.
            await Task.Delay(5);

            // if text is null or empty, show complete list
            if (string.IsNullOrEmpty(value))
                return _cities.Select(x => x.Id);

            return _cities.Where(x => x.Name.Contains(value, StringComparison.InvariantCultureIgnoreCase))
                .Select(x => x.Id);
            
        }


    }
}
