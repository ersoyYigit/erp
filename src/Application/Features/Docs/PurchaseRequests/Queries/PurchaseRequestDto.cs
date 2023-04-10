using ArdaManager.Domain.Entities.Human;
using ArdaManager.Domain.Entities.Transactions.Purchase;
using ArdaManager.Domain.Enums.Doc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Application.Features.Docs.PurchaseRequests.Queries
{
    public class PurchaseRequestDto
    {
        public int Id { get; set; }
        public string DocNo { get; set; }
        public DocState DocState { get; set; }
        public DateTime? DocDate { get; set; }
        public string Description { get; set; }
        public string RequesterName { get; set; }
        public string RequesterDepartment { get; set; }
        public string RequesterId { get; set; }
        
        public DateTime? RequirementDate { get; set; }
        public DocType DocType { get; set; }
        //public ICollection<PurchaseRequestRow> PurchaseRequestRows { get; set; }
        public ICollection<PurchaseRequestRowDto> PurchaseRequestRowsDto { get; set; }
        public string RequesterUserId { get; set; }
        public string CreatedBy { get; set; }
    }
}
