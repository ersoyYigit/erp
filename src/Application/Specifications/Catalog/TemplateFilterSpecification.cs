using ArdaManager.Application.Extensions;
using ArdaManager.Application.Specifications.Base;
using ArdaManager.Domain.Entities.Catalog;

namespace ArdaManager.Application.Specifications.Catalog
{
    public class TemplateFilterSpecification : HeroSpecification<Template>
    {
        public TemplateFilterSpecification(string searchString)
        {
            Includes.Add(a => a.Pattern);
            if (!string.IsNullOrEmpty(searchString))
            {
                Criteria = p => (p.Name.Contains(searchString) || p.Description.Contains(searchString) || p.Code.Contains(searchString) || p.Pattern.Name.Contains(searchString) 
                || p.TemplateKind.ToDescriptionStringVapp().Contains(searchString) || p.State.ToDescriptionStringVapp().Contains(searchString));
            }
            else
            {
                Criteria = p => p.Name != null;
            }
          
        }
    }
}