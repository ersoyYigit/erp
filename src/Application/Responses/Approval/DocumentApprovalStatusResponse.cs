using ArdaManager.Domain.Entities.Approval;
using ArdaManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArdaManager.Domain.Enums.Doc;

namespace ArdaManager.Application.Responses.Approval
{
    public class DocumentApprovalStatusResponse
    {
        public int Id { get; set; }
        public int BaseDocId { get; set; }
        public DocType DocType { get; set; }
        public string DocNo { get; set; }
        public DateTime? DocDate { get; set; }
        public int ApprovalStepId { get; set; }
        public int StepNumber { get; set; }
        public string UserRoleId { get; set; }
        public string UserRoleName { get; set; }
        public string UserRoleDepartment { get; set; }
        public bool IsApproved { get; set; }
        public bool IsRejected
        {
            get
            {
                return this.ApproveDate.HasValue && !IsApproved;
            }
        }

        public DateTime? ApproveDate { get; set; }
        public DocState DocState { get; set; }
    }
}
