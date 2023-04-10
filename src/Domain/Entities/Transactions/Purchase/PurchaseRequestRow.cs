using ArdaManager.Domain.Contracts;
using ArdaManager.Domain.Entities.Catalog;
using ArdaManager.Domain.Entities.Inventory;
using ArdaManager.Domain.Entities.Transactions.WarehouseDocs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Domain.Entities.Transactions.Purchase
{
    public class PurchaseRequestRow : AuditableEntity<int>
    {
        public int RowNo { get; set; }
        public int PurchaseRequestId { get; set; }
        public int ProductId { get; set; }

        [Column(TypeName = "decimal(18,8)")]
        public decimal Quantity { get; set; }
        public int MeasurementUnitId { get; set; }

        [Column(TypeName = "decimal(18,8)")]
        public decimal Price { get; set; } = decimal.Zero;

        public string Description { get; set; }



        public virtual Product Product { get; set; }
        public virtual PurchaseRequest PurchaseRequest { get; set; }
        public virtual MeasurementUnit MeasurementUnit { get; set; }
    }
}
