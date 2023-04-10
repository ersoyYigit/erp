using ArdaManager.Application.Features.Docs.TemplateWorkOrders.Commands.AddEdit;
using ArdaManager.Application.Features.Docs.TemplateWorkOrders.Commands.Delete;
using ArdaManager.Application.Features.Docs.TemplateWorkOrders.Queries.GetAll;
using ArdaManager.Application.Features.Docs.TemplateWorkOrders.Queries.GetById;
using ArdaManager.Application.Features.Racks.Queries.GetById;
using ArdaManager.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Radzen.Blazor.Rendering;
using System.Threading.Tasks;
using Path = System.IO.Path;

namespace ArdaManager.Server.Controllers.v1.Catalog
{
    public class TemplateWorkOrdersController : BaseApiController<TemplateWorkOrdersController>
    {
        /// <summary>
        /// Get All TemplateWorkOrders
        /// </summary>
        /// <returns>Status 200 OK</returns>
        
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _mediator.Send(new GetAllTemplateWorkOrdersQuery());
            return Ok(products);
        }


        /// <summary>
        /// Get a TemplateWorkOrder By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 Ok</returns>
        [Authorize(Policy = Permissions.TemplateWorkOrders.View)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var templateWorkOrder = await _mediator.Send(new GetTemplateWorkOrderByIdQuery() { Id = id });
            return Ok(templateWorkOrder);
        }



        /// <summary>
        /// Add/Edit a TemplateWorkOrder
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.TemplateWorkOrders.Create)]
        [HttpPost]
        public async Task<IActionResult> Post(AddEditTemplateWorkOrderCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        /// <summary>
        /// Delete a TemplateWorkOrder
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK response</returns>
        [Authorize(Policy = Permissions.TemplateWorkOrders.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _mediator.Send(new DeleteTemplateWorkOrderCommand { Id = id }));
        }


    }
}