using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Client.Infrastructure.Routes
{
    public static class ReportsEndpoints
    {
        public static string GetAllWarehousesStocks(int? id)
        {
            return BaseEndpoint.Server + $"api/v1/Reports/GetAllWarehousesStocks/{id}";
        }
    }
}
