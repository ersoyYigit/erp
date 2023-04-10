using ArdaManager.Application.Specifications.Base;
using ArdaManager.Domain.Entities.Human;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Application.Specifications.Human
{
    public class PersonFilterSpecification : HeroSpecification<Person>
    {
        public PersonFilterSpecification(string searchString)
        {
            
            Includes.Add(a => a.Category);
            if (!string.IsNullOrEmpty(searchString))
            {
                Criteria = p => p.Code != null && (p.Name.Contains(searchString) || p.Description.Contains(searchString) || p.Surname.Contains(searchString) || p.Code.Contains(searchString) || p.Category.Name.Contains(searchString));
            }
            else
            {
                Criteria = p => p.Code != null;
            }
        }
    }
}
