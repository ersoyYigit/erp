using ArdaManager.Application.Features.Patterns.Queries.GetAll;
using ArdaManager.Application.Features.MeasurementUnits.Queries.GetAll;
using ArdaManager.Client.Infrastructure.Extensions;
using ArdaManager.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Client.Infrastructure.Managers.Catalog.MeasurementUnit
{
    public class MeasurementUnitManager : IMeasurementUnitManager
    {
        private readonly HttpClient _httpClient;

        public MeasurementUnitManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<List<GetAllMeasurementUnitsResponse>>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync(Routes.MeasurementUnitsEndpoints.GetAll);
            return await response.ToResult<List<GetAllMeasurementUnitsResponse>>();
        }
    }
}
