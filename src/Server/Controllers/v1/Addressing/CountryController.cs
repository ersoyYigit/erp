using ArdaManager.Application.Features.Countries.Queries.GetAll;
using ArdaManager.Application.Features.Countries.Queries.GetById;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ArdaManager.Server.Controllers.v1.Addressing
{

    public class CountryController :  BaseApiController<CountryController>
    {
        /// <summary>
        /// Get All Countries
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var countries = await _mediator.Send(new GetAllCountriesQuery());
            return Ok(countries);
        }

        /// <summary>
        /// Get a Country By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 Ok</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var country = await _mediator.Send(new GetCountryByIdQuery() { Id = id });
            return Ok(country);
        }
    }
}
