namespace ArdaManager.Application.Requests.Catalog
{
    public class GetAllPagedTemplatesRequest : PagedRequest
    {
        public string SearchString { get; set; }
    }
}