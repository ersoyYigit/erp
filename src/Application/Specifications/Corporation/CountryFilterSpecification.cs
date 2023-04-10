using ArdaManager.Application.Specifications.Base;
using ArdaManager.Domain.Entities.Corporation;
using AutoFilterer.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Application.Specifications.Corporation
{
    public class CompanyFilterSpecification : HeroSpecification<Company>
    {
        
        public CompanyFilterSpecification(string searchString)
        {
            //Includes.Add(a => a.Country);
            Includes.Add(a => a.City);
            Includes.Add(a => a.City.Country);
            if (!string.IsNullOrEmpty(searchString))
            {
                Criteria = p => p.Code != null && (p.Name.Contains(searchString) || p.Name.Contains(searchString) || p.Notes.Contains(searchString) );
            }
            else
            {
                Criteria = p => p.Code != null;
            }
        }
    }
}
