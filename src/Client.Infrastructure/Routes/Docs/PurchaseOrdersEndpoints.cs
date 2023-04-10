using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Client.Infrastructure.Routes.Docs
{
    public static  class PurchaseOrdersEndpoints
    {
        public static string GetAll = BaseEndpoint.Server + "api/v1/PurchaseOrders";
        public static string Delete = BaseEndpoint.Server + "api/v1/PurchaseOrders";
        public static string Save = BaseEndpoint.Server + "api/v1/PurchaseOrders";

        public static string GetById(int id)
        {
            return BaseEndpoint.Server + $"api/v1/PurchaseOrders/GetById/{id}";
        }

    }
}
