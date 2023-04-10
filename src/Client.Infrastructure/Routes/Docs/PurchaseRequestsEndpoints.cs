using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Client.Infrastructure.Routes.Docs
{
    public static class PurchaseRequestsEndpoints
    {
        public static string GetAll = BaseEndpoint.Server + "api/v1/PurchaseRequests";
        public static string Delete = BaseEndpoint.Server + "api/v1/PurchaseRequests";
        public static string Save = BaseEndpoint.Server + "api/v1/PurchaseRequests";

        public static string GetById(int id)
        {
            return BaseEndpoint.Server + $"api/v1/PurchaseRequests/GetById/{id}";
        }


        /*
        public static string GetAllByType(PurchaseRequestType type)
        {
            return BaseEndpoint.Server + $"api/v1/PurchaseRequests/GetAllByType/{type}";
        }

        public static string GetAllByRequesterId(PurchaseRequestType type, int id)
        {
            return BaseEndpoint.Server + $"api/v1/PurchaseRequests/GetAllByRequesterId/{type}/{id}";
        }
        */
    }
}
