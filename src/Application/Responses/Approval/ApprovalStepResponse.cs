using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Application.Responses.Approval
{
    public  class ApprovalStepResponse
    {
        public int Id { get; set; }
        public int StepNumber { get; set; }
        public string UserRoleId { get; set; }
        public string UserRoleName { get; set; }
    }
}
