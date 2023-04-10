namespace ArdaManager.Client.Infrastructure.Routes
{
    public static class ProductCategoriesEndpoints
    {
        public static string ExportFiltered(string searchString)
        {
            return BaseEndpoint.Server + $"{Export}?searchString={searchString}";
        }

        public static string Export = BaseEndpoint.Server + "api/v1/productCategories/export";

        public static string GetAll = BaseEndpoint.Server + "api/v1/productCategories";
        public static string Delete = BaseEndpoint.Server + "api/v1/productCategories";
        public static string Save = BaseEndpoint.Server + "api/v1/productCategories";
        public static string GetCount = BaseEndpoint.Server + "api/v1/productCategories/count";
    }
}