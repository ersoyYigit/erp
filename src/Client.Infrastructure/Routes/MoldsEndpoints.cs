using System.Linq;

namespace ArdaManager.Client.Infrastructure.Routes
{
    public static class MoldsEndpoints
    {
        public static string GetAllPaged(int pageNumber, int pageSize, string searchString, string[] orderBy)
        {
            var url = BaseEndpoint.Server + $"api/v1/molds?pageNumber={pageNumber}&pageSize={pageSize}&searchString={searchString}&orderBy=";
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

        public static string GetCount = BaseEndpoint.Server + "api/v1/molds/count";

        public static string GetMoldImage(int moldId)
        {
            return BaseEndpoint.Server + $"api/v1/molds/image/{moldId}";
        }

        public static string ExportFiltered(string searchString)
        {
            return BaseEndpoint.Server + $"{Export}?searchString={searchString}";
        }

        public static string Save = BaseEndpoint.Server + "api/v1/molds";
        public static string Delete = BaseEndpoint.Server + "api/v1/molds";
        public static string Export = BaseEndpoint.Server + "api/v1/molds/export";
    }
}