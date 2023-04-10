using ArdaManager.Domain.Entities.Corporation;
using System;
using System.Collections.Generic;
using ArdaManager.Domain.Enums.Doc;
using System.ComponentModel.DataAnnotations.Schema;
using ArdaManager.Domain.Entities.Misc;

namespace ArdaManager.Domain.Entities.Transactions.Purchase
{
    [Table("PurchaseOrders")]
    public class PurchaseOrder : BaseDoc
    {
        public PurchaseOrder()
        {
            this.DocType = DocType.PurchaseOrder;
            ExchangeDate = DateTime.Now;
            PurchaseOrderRows = new HashSet<PurchaseOrderRow>();
        }

        public DateTime? RequirementDate { get; set; }
        public string RequesterId { get; set; }
        public string RequesterName { get; set; }
        public string RequesterDepartment { get; set; }

        public int CompanyId { get; set; }
        public int CurrencyId { get; set; }
        public DateTime ExchangeDate { get; set; }

        [Column(TypeName = "decimal(18,8)")]
        public decimal ExchangeRate { get; set; } = decimal.Zero;

        public int? PurchaseRequestId { get; set; }
        public int? PurchaseOfferId { get; set; }


        public virtual PurchaseOffer PurchaseOffer { get; set; }
        public virtual PurchaseRequest PurchaseRequest { get; set; }
        public virtual Company Company { get; set; }
        public virtual Currency Currency { get; set; }
        public virtual ICollection<PurchaseOrderRow> PurchaseOrderRows { get; set; }
    }
}
