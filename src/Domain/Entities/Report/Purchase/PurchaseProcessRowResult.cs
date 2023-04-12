using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Domain.Entities.Report.Purchase
{
    public  class PurchaseProcessRowResult
    {
        public int PurchaseRequestId { get; set; }
        public int ProductId { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public decimal PurchaseRequestQuantity { get; set; }
        public string ProductMeasurementUnit { get; set; }
        public decimal PurchaseOfferQuantity { get; set; } = decimal.Zero;
        public decimal PurchaseOfferPrice { get; set; } = decimal.Zero;
        public string PurchaseOfferCurrencyCode { get; set; }
        public decimal PurchaseOfferExchangeRate { get; set; } = decimal.Zero;
        public string PurchaseOfferDescription { get; set; }
        public decimal PurchaseOrderQuantity { get; set; } = decimal.Zero;
        public decimal PurchaseOrderPrice { get; set; } = decimal.Zero;
        public string PurchaseOrderCurrencyCode { get; set; }
        public decimal PurchaseOrderExchangeRate { get; set; } = decimal.Zero;
        public string PurchaseOrderDescription { get; set; }
        public decimal WarehouseEntryQuantity { get; set; } = decimal.Zero;
        public string WarehouseEntryDescription { get; set; }
    }
}
