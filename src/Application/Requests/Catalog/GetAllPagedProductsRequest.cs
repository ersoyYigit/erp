namespace ArdaManager.Application.Requests.Catalog
{
    public class GetAllPagedProductsRequest : PagedRequest
    {
        public string SearchString { get; set; }
        public bool isTemplate { get; set; }
    }
}