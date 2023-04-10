using ArdaManager.Domain.Entities.Corporation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using ArdaManager.Domain.Enums.Doc;
using ArdaManager.Domain.Entities.Misc;

namespace ArdaManager.Domain.Entities.Transactions.Purchase
{
    [Table("PurchaseOffers")]
    public class PurchaseOffer : BaseDoc
    {
        public PurchaseOffer()
        {
            this.DocType = DocType.PurchaseOffer;
            ExchangeDate = DateTime.Now;
            PurchaseOfferRows = new HashSet<PurchaseOfferRow>();
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


        public virtual PurchaseRequest PurchaseRequest { get; set; }
        public virtual Company Company { get; set; }

        public virtual Currency Currency { get; set; }
        public virtual ICollection<PurchaseOfferRow> PurchaseOfferRows { get; set; }
    }
}
