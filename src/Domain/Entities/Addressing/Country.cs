using ArdaManager.Domain.Contracts;
using ArdaManager.Domain.Entities.Corporation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Domain.Entities.Addressing
{
    public  class Country : AuditableEntity<int>
    {

        public string Name { get; set; }
        public string Code { get; set; }
        public virtual ICollection<City> Cities { get; set; }
        //public virtual ICollection<Company> Companies { get; set; }
    }
}
