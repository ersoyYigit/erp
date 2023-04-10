using ArdaManager.Application.Features.MeasurementUnits.Queries.GetAll;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ArdaManager.Server.Controllers.v1.Catalog
{
    public class MeasurementUnitsController : BaseApiController<MeasurementUnitsController>
    {
        /// <summary>
        /// Get All Patterns
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var patterns = await _mediator.Send(new GetAllMeasurementUnitsQuery());
            return Ok(patterns);
        }
    }
}
