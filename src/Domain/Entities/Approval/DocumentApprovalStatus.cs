using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Domain.Entities.Approval
{
    public class DocumentApprovalStatus
    {
        public int Id { get; set; }
        public int BaseDocId { get; set; }
        public BaseDoc BaseDoc { get; set; }
        public int ApprovalStepId { get; set; }
        public ApprovalStep ApprovalStep { get; set; }
        public bool IsApproved { get; set; }
        public DateTime? ApproveDate { get; set; }
    }
}
