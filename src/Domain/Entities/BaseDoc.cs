using ArdaManager.Domain.Contracts;
using ArdaManager.Domain.Entities.Approval;
using ArdaManager.Domain.Entities.Misc;
using ArdaManager.Domain.Enums.Doc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Domain.Entities
{

    public class BaseDoc : AuditableEntity<int> , IBaseDoc
    {
        public string DocNo { get; set; }
        public DocType DocType { get; set; } = DocType.Empty;
        public DocState DocState { get; set; } = DocState.Waiting;
        public Guid Guid { get; set; } = Guid.NewGuid();
        public DateTime? DocDate { get; set; } = DateTime.UtcNow;
        public string Description { get; set; }

        public int? PrevDocId { get; set; }
        public int? NextDocId { get; set; }

        public virtual BaseDoc PrevDoc { get; set; }
        public virtual BaseDoc NextDoc { get; set; }
        public virtual ICollection<DocumentApprovalStatus> ApprovalStatuses { get; set; }
    }
}
