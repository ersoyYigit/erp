using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Client.Infrastructure.Routes
{
    public static class PersonsEndpoints
    {
        public static string GetAllPaged(int pageNumber, int pageSize, string searchString, string[] orderBy)
        {
            var url = BaseEndpoint.Server + $"api/v1/persons?pageNumber={pageNumber}&pageSize={pageSize}&searchString={searchString}&orderBy=";
            if (orderBy?.Any() == true)
            {
                foreach (var orderByPart in orderBy)
                {
                    url += $"{orderByPart},";
                }
                url = url[..^1]; // loose training ,
            }
            return BaseEndpoint.Server + url;
        }

        public static string GetCount = BaseEndpoint.Server + "api/v1/persons/count";

        public static string GetPersonImage(int personId)
        {
            return BaseEndpoint.Server + $"api/v1/persons/image/{personId}";
        }

        public static string ExportFiltered(string searchString)
        {
            return BaseEndpoint.Server + $"{Export}?searchString={searchString}";
        }

        public static string Save = BaseEndpoint.Server + "api/v1/persons";
        public static string Delete = BaseEndpoint.Server + "api/v1/persons";
        public static string Export = BaseEndpoint.Server + "api/v1/persons/export";
    }
}
