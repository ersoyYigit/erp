using ArdaManager.Application.Features.Reports.Warehouses;
using ArdaManager.Domain.Entities.Report.Warehouse;
using ArdaManager.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Client.Infrastructure.Managers.Reporting
{
    public interface IReportsManager : IManager
    {
        Task<IResult<List<WarehouseReport>>> GetAllWarehousesStocks(GetAllWarehousesStocksQuery query);
    }
}
