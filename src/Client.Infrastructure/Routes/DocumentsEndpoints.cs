namespace ArdaManager.Client.Infrastructure.Routes
{
    public static class DocumentsEndpoints
    {
        public static string GetAllPaged(int pageNumber, int pageSize, string searchString)
        {
            return BaseEndpoint.Server + $"api/documents?pageNumber={pageNumber}&pageSize={pageSize}&searchString={searchString}";
        }

        public static string GetById(int documentId)
        {
            return BaseEndpoint.Server + $"api/documents/{documentId}";
        }

        public static string Save = BaseEndpoint.Server + "api/documents";
        public static string Delete = BaseEndpoint.Server + "api/documents";
    }
}