using ArdaManager.Application.Features.Persons.Commands.AddEdit;
using ArdaManager.Application.Features.Persons.Commands.Delete;
using ArdaManager.Application.Features.Persons.Queries.GetAllPaged;
using ArdaManager.Application.Features.Persons.Queries.GetPersonImage;
using ArdaManager.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ArdaManager.Server.Controllers.v1.Human
{
    public class PersonsController : BaseApiController<PersonsController>
    {
        /// <summary>
        /// Get All Persons
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchString"></param>
        /// <param name="orderBy"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Persons.View)]
        [HttpGet]
        public async Task<IActionResult> GetAll(int pageNumber, int pageSize, string searchString, string orderBy = null)
        {
            var persons = await _mediator.Send(new GetAllPersonsQuery(pageNumber, pageSize, searchString, orderBy));
            return Ok(persons);
        }

        /// <summary>
        /// Get a Person Image by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Persons.View)]
        [HttpGet("image/{id}")]
        public async Task<IActionResult> GetPersonImageAsync(int id)
        {
            var result = await _mediator.Send(new GetPersonImageQuery(id));
            return Ok(result);
        }

        /// <summary>
        /// Add/Edit a Person
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Persons.Create)]
        [HttpPost]
        public async Task<IActionResult> Post(AddEditPersonCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        /// <summary>
        /// Delete a Person
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK response</returns>
        [Authorize(Policy = Permissions.Persons.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _mediator.Send(new DeletePersonCommand { Id = id }));
        }

        
    }
}
