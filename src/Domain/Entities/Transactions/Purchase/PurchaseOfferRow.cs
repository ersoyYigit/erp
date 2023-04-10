using ArdaManager.Domain.Contracts;
using ArdaManager.Domain.Entities.Catalog;
using ArdaManager.Domain.Entities.Misc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Domain.Entities.Transactions.Purchase
{
    public class PurchaseOfferRow : AuditableEntity<int>
    {

        public int RowNo { get; set; }
        public int PurchaseOfferId { get; set; }
        public int ProductId { get; set; }

        [Column(TypeName = "decimal(18,8)")]
        public decimal Quantity { get; set; }
        public int MeasurementUnitId { get; set; }
        public string Description { get; set; }
        public int? PrevRowId { get; set; }
        public int? PurchaseRequestRowId { get; set; }



        /* PRICE PROPERTIES*/
        [Column(TypeName = "decimal(18,8)")]
        public decimal Price { get; set; } = decimal.Zero;
        public int CurrencyId { get; set; }

        [Column(TypeName = "decimal(18,8)")]
        public decimal ExchangeRate { get; set; } = decimal.Zero;
        public int TaxId { get; set; }
        
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





        public virtual PurchaseRequestRow PurchaseRequestRow { get; set; }
        public virtual Product Product { get; set; }
        public virtual PurchaseOffer PurchaseOffer { get; set; }
        public virtual MeasurementUnit MeasurementUnit { get; set; }
    }
}
