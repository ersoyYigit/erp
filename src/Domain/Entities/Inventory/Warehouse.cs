using ArdaManager.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Domain.Entities.Inventory
{
    [Table("Warehouses")]
    public class Warehouse : BaseEntity
    {
        public string Description { get; set; }

        public virtual ICollection<Rack> Racks { get; set; }

    }
}
