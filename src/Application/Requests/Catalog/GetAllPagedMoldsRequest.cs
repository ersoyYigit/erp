namespace ArdaManager.Application.Requests.Catalog
{
    public class GetAllMoldsRequest : PagedRequest
    {
        public string SearchString { get; set; }
    }
}