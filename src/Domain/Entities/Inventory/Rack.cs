using ArdaManager.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Domain.Entities.Inventory
{
    public class Rack : AuditableEntity<int>
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Section { get; set; }
        public string SectionCode { get; set; }
        public string Description { get; set; }

        public int WarehouseId { get; set; }
        public Warehouse Warehouse { get; set; }
    }
}
