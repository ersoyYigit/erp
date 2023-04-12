using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Domain.Entities.Report.Purchase
{
    public class PurchaseProcessResult
    {

        public int PurchaseRequestId { get; set; }
        public string PurchaseRequestDocNo { get; set; }
        public DateTime PurchaseRequestDocDate { get; set; }
        public string RequesterName { get; set; }
        public string RequesterDepartment { get; set; }
        public int? PurchaseOfferId { get; set; }
        public string PurchaseOfferDocNo { get; set; }
        public DateTime? PurchaseOfferDocDate { get; set; }
        public string PurchaseOfferCompanyName { get; set; }
        public decimal PurchaseOfferSum { get; set; } = decimal.Zero;
        public string PurchaseOfferCurrencyCode { get; set; }
        public int? PurchaseOrderId { get; set; }
        public string PurchaseOrderDocNo { get; set; }
        public DateTime? PurchaseOrderDocDate { get; set; }
        public string PurchaseOrderCompanyName { get; set; }
        public decimal PurchaseOrderSum { get; set; } = decimal.Zero;
        public string PurchaseOrderCurrencyCode { get; set; }
        public int? WarehouseEntryId { get; set; }
        public string WarehouseEntryDocNo { get; set; }
        public DateTime? WarehouseEntryDocDate { get; set; }
        public string WarehouseName { get; set; }
    }
}
