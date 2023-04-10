using ArdaManager.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Domain.Entities.Misc
{
    public class Currency : AuditableEntity<int>
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Sign { get; set; }
        public string CustomCode { get; set; }
    }
}
