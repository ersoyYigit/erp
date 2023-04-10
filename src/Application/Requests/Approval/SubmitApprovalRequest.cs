using ArdaManager.Domain.Enums.Doc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Application.Requests.Approval
{
    public class SubmitApprovalRequest
    {
        public int DocumentId { get; set; }
        public DocType DocType { get; set; }
        public string UserId { get; set; }
    }
}
