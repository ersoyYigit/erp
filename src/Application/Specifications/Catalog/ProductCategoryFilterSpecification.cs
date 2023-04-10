using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArdaManager.Application.Specifications.Base;
using ArdaManager.Domain.Entities.Catalog;

namespace ArdaManager.Application.Specifications.Catalog
{
    
    public class ProductCategoryFilterSpecification : HeroSpecification<ProductCategory>
    {
        public ProductCategoryFilterSpecification(string searchString)
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
