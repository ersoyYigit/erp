namespace ArdaManager.Application.Features.Patterns.Queries.GetById
{
    public class GetPatternByIdResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Tax { get; set; }
        public string Description { get; set; }
    }
}