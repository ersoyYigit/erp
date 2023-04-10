using System;
using System.Linq;

namespace ArdaManager.Client.Infrastructure.Routes
{
    public static class TemplateWorkOrdersEndpoints
    {
        public static string GetAll = BaseEndpoint.Server + "api/v1/TemplateWorkOrders";
        public static string Delete = BaseEndpoint.Server + "api/v1/TemplateWorkOrders";
        public static string Save = BaseEndpoint.Server + "api/v1/TemplateWorkOrders";
        //public static string GetById = BaseEndpoint.Server + "api/v1/TemplateWorkOrders";
        public static string GetById(int id)
        {
            return BaseEndpoint.Server + $"api/v1/TemplateWorkOrders/GetById/{id}";
        }


    }
}