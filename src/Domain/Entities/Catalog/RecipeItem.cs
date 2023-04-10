using ArdaManager.Domain.Contracts;
using ArdaManager.Domain.Entities.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Domain.Entities.Catalog
{
    public class RecipeItem : AuditableEntity<int>
    {
        public int OwnerProductId { get; set; }
        public int RecipeProductId { get; set; }
        public int WarehouseId { get; set; }
        public decimal Quantity { get; set; }
        public string Description { get; set; }

        public virtual Product OwnerProduct { get; set; }
        public virtual Product RecipeProduct { get; set; }
        public virtual Warehouse Warehouse { get; set; }
    }
}
