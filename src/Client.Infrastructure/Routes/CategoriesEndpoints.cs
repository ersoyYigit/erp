using ArdaManager.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Client.Infrastructure.Routes
{
    public static class CategoriesEndpoints
    {

        public static string GetAll = BaseEndpoint.Server + "api/v1/Categories";
        public static string Delete = BaseEndpoint.Server + "api/v1/Categories";
        public static string Save = BaseEndpoint.Server + "api/v1/Categories";
        public static string AddEdit = BaseEndpoint.Server + "api/v1/Categories/AddEdit";
        public static string GetById = BaseEndpoint.Server + "api/v1/Categories";
        public static string GetByType(int type)
        {
            return BaseEndpoint.Server + $"api/v1/categories/GetByType/{type}";
        }

        
    }
}
