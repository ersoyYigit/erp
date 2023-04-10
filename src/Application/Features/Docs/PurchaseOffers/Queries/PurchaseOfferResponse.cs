using ArdaManager.Application.Features.Docs.PurchaseRequests.Queries;
using ArdaManager.Domain.Entities.Misc;
using ArdaManager.Domain.Enums.Doc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Application.Features.Docs.PurchaseOffers.Queries
{
    public class PurchaseOfferResponse
    {

        public int Id { get; set; }
        public string DocNo { get; set; }
        public DateTime? DocDate { get; set; }
        public DocState DocState { get; set; }
        public string Description { get; set; }
        public string RequesterName { get; set; }
        public string RequesterId { get; set; }
        public string RequesterDepartment { get; set; }
        public DateTime? RequirementDate { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public int? PurchaseRequestId { get; set; }
        public string PurchaseRequestDocNo { get; set; }

        public DocType DocType { get; set; }
        public ICollection<PurchaseOfferRowResponse> PurchaseOfferRowResponse { get; set; }
        public string RequesterUserId { get; set; }
        public string CreatedBy { get; set; }

        public int? PrevDocId { get; set; }
        public int? NextDocId { get; set; }

        public int CurrencyId { get; set; }
        public string CurrencyName { get; set; }
        public string CurrencyCode { get; set; }

        public DateTime ExchangeDate { get; set; }
        
        public decimal ExchangeRate { get; set; }

        public virtual Currency Currency { get; set; }
        public virtual BaseDocResponse PrevDoc { get; set; }
        public virtual BaseDocResponse NextDoc { get; set; }
    }
}
