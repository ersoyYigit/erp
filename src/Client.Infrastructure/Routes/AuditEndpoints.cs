namespace ArdaManager.Client.Infrastructure.Routes
{
    public static class AuditEndpoints
    {
        public static string DownloadFileFiltered(string searchString, bool searchInOldValues = false, bool searchInNewValues = false)
        {
            return BaseEndpoint.Server + $"{DownloadFile}?searchString={searchString}&searchInOldValues={searchInOldValues}&searchInNewValues={searchInNewValues}";
        }

        public static string GetCurrentUserTrails = BaseEndpoint.Server + "api/audits";
        public static string DownloadFile = BaseEndpoint.Server + "api/audits/export";
    }
}