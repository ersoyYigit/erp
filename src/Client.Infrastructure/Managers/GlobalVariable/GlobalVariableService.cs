using ArdaManager.Application.Features.Categories.Queries.GetAll;
using ArdaManager.Application.Features.Currencies.Queries;
using ArdaManager.Application.Features.MeasurementUnits.Queries.GetAll;
using ArdaManager.Application.Features.Taxes.Queries;
using ArdaManager.Application.Responses.Identity;
using ArdaManager.Client.Infrastructure.Extensions;
using ArdaManager.Client.Infrastructure.Managers.Catalog.MeasurementUnit;
using ArdaManager.Client.Infrastructure.Managers.Identity.Users;
using ArdaManager.Client.Infrastructure.Managers.Misc.Category;
using ArdaManager.Domain.Entities.Catalog;
using ArdaManager.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Client.Infrastructure.Managers.GlobalVariable
{
    public  class GlobalVariableService : IGlobalVariableService
    {
        
        
        private readonly HttpClient _httpClient;


        

        public List<UserResponse> AppUsers { get; set; }
        //public List<GetAllCategoriesResponse> Categories { get; set; }
        public List<GetAllMeasurementUnitsResponse> MeasurementUnits { get; set; }
        public List<GetAllCurrenciesResponse> Currencies { get; set; }
        public List<GetAllTaxesResponse> Taxes { get; set; }

        public GlobalVariableService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            LoadMeasurementUnitsListAsync();
            LoadUserListAsync();
            LoadCurrenciesAsync();
            LoadTaxesAsync();
        }



        private async void LoadTaxesAsync()
        {
            var response = await GetAllTaxesAsync();
            if (response.Succeeded)
            {
                Taxes = response.Data;
            }
        }

        private async Task<IResult<List<GetAllTaxesResponse>>> GetAllTaxesAsync()
        {
            var response = await _httpClient.GetAsync(Routes.TaxesEndpoints.GetAll);
            return await response.ToResult<List<GetAllTaxesResponse>>();
        }

        private async void LoadCurrenciesAsync()
        {
            var response = await GetAllCurrenciesAsync();
            if (response.Succeeded)
            {
                Currencies = response.Data;
            }
        }

        private async Task<IResult<List<GetAllCurrenciesResponse>>> GetAllCurrenciesAsync()
        {
            var response = await _httpClient.GetAsync(Routes.CurrenciesEndpoints.GetAll);
            return await response.ToResult<List<GetAllCurrenciesResponse>>();
        } 



        private async void LoadMeasurementUnitsListAsync()
        {
            var response = await GetAllMuAsync();
            if (response.Succeeded)
            {
                MeasurementUnits = response.Data;
            }
        }

        private async Task<IResult<List<GetAllMeasurementUnitsResponse>>> GetAllMuAsync()
        {
            var response = await _httpClient.GetAsync(Routes.MeasurementUnitsEndpoints.GetAll);
            return await response.ToResult<List<GetAllMeasurementUnitsResponse>>();
        }


        private async void LoadUserListAsync()
        {
            var result = await GetAllUsersAsync();
            if (result.Succeeded)
            {
                AppUsers = result.Data;
            }
        }
        public async Task<IResult<List<UserResponse>>> GetAllUsersAsync()
        {
            var response = await _httpClient.GetAsync(Routes.UserEndpoints.GetAll);
            return await response.ToResult<List<UserResponse>>();
        }

    }
}
