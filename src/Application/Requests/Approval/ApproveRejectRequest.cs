using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Application.Requests.Approval
{
    public class ApproveRejectRequest
    {
        public int DocumentId { get; set; }
        public int StepNumber { get; set; } 
        public string UserId { get; set; }
    }
}
