using ArdaManager.Application.Specifications.Base;
using ArdaManager.Domain.Entities.Catalog;

namespace ArdaManager.Application.Specifications.Catalog
{
    public class ProductFilterSpecification : HeroSpecification<Product>
    {
        public ProductFilterSpecification(string searchString)
        {
            Includes.Add(a => a.Pattern);
            Includes.Add(a => a.Category);
            if (!string.IsNullOrEmpty(searchString))
            {
                Criteria = p => (p.Name.Contains(searchString) || p.Description.Contains(searchString) || p.Code.Contains(searchString) || p.Pattern.Name.Contains(searchString));
            }
            else
            {
                Criteria = p => p.Name != null;
            }
            

        }
    }
}