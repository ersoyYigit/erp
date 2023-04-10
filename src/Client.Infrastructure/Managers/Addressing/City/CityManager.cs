using ArdaManager.Application.Features.Cities.Queries.GetAll;
using ArdaManager.Application.Features.Cities.Queries.GetById;
using ArdaManager.Application.Features.Cities.Queries.GetCitiesWithCountryId;
using ArdaManager.Client.Infrastructure.Extensions;
using ArdaManager.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Client.Infrastructure.Managers.Addressing.City
{
    public class CityManager : ICityManager
    {
        private readonly HttpClient _httpClient;

        public CityManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<List<GetAllCitiesResponse>>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync(Routes.CitiesEndpoints.GetAll());
            return await response.ToResult<List<GetAllCitiesResponse>>();
        }


        public async Task<IResult<List<GetCityByIdResponse>>> GetCityByIdAsync(GetCityByIdQuery query)
        {
            var response = await _httpClient.GetAsync(Routes.CitiesEndpoints.GetById(query.Id));
            return await response.ToResult<List<GetCityByIdResponse>>();
        }


        public async Task<IResult<List<GetCitiesByCountryIdResponse>>> GetCitiesByCountryIdAsync(GetCitiesByCountryIdQuery query)
        {
            var response = await _httpClient.GetAsync(Routes.CitiesEndpoints.GetByCountryIdAll(query.CountryId));
            return await response.ToResult<List<GetCitiesByCountryIdResponse>>();
        }
    }
}
