using ArdaManager.Domain.Contracts;
using ArdaManager.Domain.Entities.Catalog;
using ArdaManager.Domain.Entities.Inventory;
using ArdaManager.Domain.Entities.Misc;
using ArdaManager.Domain.Entities.Transactions.Purchase;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Domain.Entities.Transactions.WarehouseDocs
{
    public  class WarehouseRequestRow : AuditableEntity<int>
    {
        public int RowNo { get; set; }
        public int WarehouseRequestId { get; set; }
        public int ProductId { get; set; }

        [Column(TypeName = "decimal(18,8)")]
        public decimal Quantity { get; set; }
        public int MeasurementUnitId { get; set; }


        public string Description { get; set; }
        public int? RackId { get; set; }

        public virtual Product Product { get; set; }
        public virtual WarehouseRequest WarehouseRequest { get; set; }
        public virtual MeasurementUnit MeasurementUnit { get; set; }
        public virtual Rack Rack { get; set; }
    }
}
