using ArdaManager.Domain.Contracts;
using ArdaManager.Domain.Entities.Corporation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Domain.Entities.Addressing
{
    public class City : AuditableEntity<int>
    {

        public string Name { get; set; }
        public string Code { get; set; }


        public int CountryId { get; set; }
        public virtual Country Country { get; set; }

        public virtual ICollection<Company> Companies { get; set; }

    }
}
