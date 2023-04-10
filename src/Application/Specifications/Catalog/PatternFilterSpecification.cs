using ArdaManager.Application.Specifications.Base;
using ArdaManager.Domain.Entities.Catalog;

namespace ArdaManager.Application.Specifications.Catalog
{
    public class PatternFilterSpecification : HeroSpecification<Pattern>
    {
        public PatternFilterSpecification(string searchString)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                Criteria = p => p.Name.Contains(searchString) || p.Description.Contains(searchString);
            }
            else
            {
                Criteria = p => true;
            }
        }
    }
}
