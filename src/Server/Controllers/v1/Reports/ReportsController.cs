using ArdaManager.Application.Features.Docs.TemplateWorkOrders.Queries.GetAll;
using ArdaManager.Application.Features.Reports.Warehouses;
using ArdaManager.Server.Controllers.v1.Catalog;
using ArdaManager.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Polly;
using System.Threading.Tasks;

namespace ArdaManager.Server.Controllers.v1.Reports
{
    public class ReportsController : BaseApiController<ReportsController>
    {
        /// <summary>
        /// Get All TemplateWorkOrders
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Reports.GetAllWarehousesStocks)]
        [HttpGet("GetAllWarehousesStocks/{id}")]
        public async Task<IActionResult> GetAllWarehousesStocks(int id)
        {
            var products = await _mediator.Send(new GetAllWarehousesStocksQuery() { WarehouseId = id });
            return Ok(products);
        }
    }
}
