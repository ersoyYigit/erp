using ArdaManager.Application.Features.Reports.Warehouses;
using ArdaManager.Client.Infrastructure.Extensions;
using ArdaManager.Client.Infrastructure.Managers.Docs.TemplateWorkOrder;
using ArdaManager.Client.Infrastructure.Routes;
using ArdaManager.Domain.Entities.Report.Warehouse;
using ArdaManager.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Client.Infrastructure.Managers.Reporting
{
    public  class ReportsManager : IReportsManager
    {
        private readonly HttpClient _httpClient;

        public ReportsManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }



        public async Task<IResult<List<WarehouseReport>>> GetAllWarehousesStocks(GetAllWarehousesStocksQuery query)
        {
            var response = await _httpClient.GetAsync(ReportsEndpoints.GetAllWarehousesStocks(query.WarehouseId));
            return await response.ToResult< List<WarehouseReport>>();
        }
    }
}
