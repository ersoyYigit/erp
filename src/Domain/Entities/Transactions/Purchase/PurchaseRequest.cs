using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArdaManager.Domain.Entities.Human;
using ArdaManager.Domain.Enums.Doc;

namespace ArdaManager.Domain.Entities.Transactions.Purchase
{

    [Table("PurchaseRequests")]
    public class PurchaseRequest : BaseDoc
    {
        public PurchaseRequest()
        {
            this.DocType = DocType.PurchaseRequest;
            PurchaseRequestRows = new HashSet<PurchaseRequestRow>();
        }


        public DateTime? RequirementDate { get; set; }
        public string RequesterId { get; set; }
        public string RequesterName { get; set; }
        public string RequesterDepartment { get; set; }

        
        public virtual ICollection<PurchaseRequestRow> PurchaseRequestRows { get; set; }
    }

}

