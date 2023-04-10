using ArdaManager.Application.Features.Countries.Queries.GetAll;
using ArdaManager.Client.Infrastructure.Extensions;
using ArdaManager.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Client.Infrastructure.Managers.Addressing.Country
{

    public class CountryManager : ICountryManager
    {
        private readonly HttpClient _httpClient;

        public CountryManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<List<GetAllCountriesResponse>>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync(Routes.CountriesEndpoints.GetAll);
            return await response.ToResult<List<GetAllCountriesResponse>>();
        }


    }
}
