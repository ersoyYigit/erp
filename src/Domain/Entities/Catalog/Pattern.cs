using ArdaManager.Domain.Contracts;

namespace ArdaManager.Domain.Entities.Catalog
{
    public class Pattern : AuditableEntity<int>
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}