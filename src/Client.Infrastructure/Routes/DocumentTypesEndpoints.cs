namespace ArdaManager.Client.Infrastructure.Routes
{
    public static class DocumentTypesEndpoints
    {
        public static string ExportFiltered(string searchString)
        {
            return BaseEndpoint.Server + $"{Export}?searchString={searchString}";
        }

        public static string Export = BaseEndpoint.Server + "api/documentTypes/export";

        public static string GetAll = BaseEndpoint.Server + "api/documentTypes";
        public static string Delete = BaseEndpoint.Server + "api/documentTypes";
        public static string Save = BaseEndpoint.Server + "api/documentTypes";
        public static string GetCount = BaseEndpoint.Server + "api/documentTypes/count";
    }
}