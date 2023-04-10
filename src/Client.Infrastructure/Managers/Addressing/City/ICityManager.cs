using ArdaManager.Application.Features.Cities.Queries.GetAll;
using ArdaManager.Application.Features.Cities.Queries.GetById;
using ArdaManager.Application.Features.Cities.Queries.GetCitiesWithCountryId;
using ArdaManager.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Client.Infrastructure.Managers.Addressing.City
{
    public interface ICityManager : IManager
    {
        Task<IResult<List<GetAllCitiesResponse>>> GetAllAsync();
        Task<IResult<List<GetCityByIdResponse>>> GetCityByIdAsync(GetCityByIdQuery query);
        Task<IResult<List<GetCitiesByCountryIdResponse>>> GetCitiesByCountryIdAsync(GetCitiesByCountryIdQuery query);

    }
}
