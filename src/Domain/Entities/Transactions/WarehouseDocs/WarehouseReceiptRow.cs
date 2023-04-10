using ArdaManager.Domain.Contracts;
using ArdaManager.Domain.Entities.Catalog;
using ArdaManager.Domain.Entities.Corporation;
using ArdaManager.Domain.Entities.Inventory;
using ArdaManager.Domain.Entities.Misc;
using ArdaManager.Domain.Entities.Transactions.Purchase;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Domain.Entities.Transactions.WarehouseDocs
{
    public class WarehouseReceiptRow : AuditableEntity<int>
    {
        public int RowNo { get; set; }
        public int WarehouseReceiptId { get; set; }
        public int ProductId { get; set; }

        [Column(TypeName = "decimal(18,8)")]
        public decimal Quantity { get; set; }
        public int MeasurementUnitId { get; set; }
        public int? RackId { get; set; }

        public string Description { get; set; }


        /* PRICE PROPERTIES*/
        [Column(TypeName = "decimal(18,8)")]
        public decimal Price { get; set; } = decimal.Zero;
        public int? CurrencyId { get; set; }

        [Column(TypeName = "decimal(18,8)")]
        public decimal ExchangeRate { get; set; } = decimal.Zero;
        public int? TaxId { get; set; }

        [Column(TypeName = "decimal(18,8)")]
        public decimal TaxAmount { get; set; } = decimal.Zero;

        [Column(TypeName = "decimal(18,2)")]
        public decimal DiscountPercentage { get; set; }

        [Column(TypeName = "decimal(18,8)")]
        public decimal DiscountAmount { get; set; } = decimal.Zero;

        [Column(TypeName = "decimal(18,8)")]
        public decimal TotalAmount { get; set; } = decimal.Zero;

        public virtual Currency Currency { get; set; }
        public virtual Tax Tax { get; set; }
        /* PRICE PROPERTIES*/

        public int? PurchaseOrderRowId { get; set; }

        public virtual PurchaseOrderRow PurchaseOrderRow { get; set; }
        public virtual Product Product { get; set; }
        public virtual WarehouseReceipt WarehouseReceipt { get; set; }
        public virtual MeasurementUnit MeasurementUnit { get; set; }
        public virtual Rack Rack { get; set; }
    }
}
