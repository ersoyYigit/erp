using ArdaManager.Domain.Enums.Doc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Client.Infrastructure.Routes.Docs
{
    public static class WarehouseReceiptsEndpoints
    {
        public static string GetAll = BaseEndpoint.Server + "api/v1/WarehouseReceipts";
        public static string Delete = BaseEndpoint.Server + "api/v1/WarehouseReceipts";
        public static string Save = BaseEndpoint.Server + "api/v1/WarehouseReceipts";
        //public static string GetById = BaseEndpoint.Server + "api/v1/WarehouseReceipts";
        public static string GetById(int id)
        {
            return BaseEndpoint.Server + $"api/v1/WarehouseReceipts/GetById/{id}";
        }


        public static string GetAllByType(WarehouseReceiptType type)
        {
            return BaseEndpoint.Server + $"api/v1/WarehouseReceipts/GetAllByType/{type}";
        }

        public static string GetAllByWarehouseId(WarehouseReceiptType type, int id)
        {
            return BaseEndpoint.Server + $"api/v1/WarehouseReceipts/GetAllByWarehouseId/{type}/{id}";
        }
    }
}
