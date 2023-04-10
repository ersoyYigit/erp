using ArdaManager.Application.Features.Cities.Queries.GetAll;
using ArdaManager.Application.Features.Cities.Queries.GetById;
using ArdaManager.Application.Features.Cities.Queries.GetCitiesWithCountryId;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ArdaManager.Server.Controllers.v1.Addressing
{
    public class CityController : BaseApiController<CityController>
    {
        /// <summary>
        /// Get All Cities
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var cities = await _mediator.Send(new GetAllCitiesQuery());
            return Ok(cities);
        }

        /// <summary>
        /// Get a City By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 Ok</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var city = await _mediator.Send(new GetCityByIdQuery() { Id = id });
            return Ok(city);
        }


        /// <summary>
        /// Get a City By Country Id
        /// </summary>
        /// <param name="countryid"></param>
        /// <returns>Status 200 Ok</returns>
        [HttpGet("GetByCountryId/{countryid}")]
        public async Task<IActionResult> GetByCountryId(int countryid)
        {
            var cities = await _mediator.Send(new GetCitiesByCountryIdQuery() { CountryId = countryid });
            return Ok(cities);
        }

    }
}
