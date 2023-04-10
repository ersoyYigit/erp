using ArdaManager.Domain.Contracts;

namespace ArdaManager.Domain.Entities.Catalog
{
    public class ProductCategory : AuditableEntity<int>
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
