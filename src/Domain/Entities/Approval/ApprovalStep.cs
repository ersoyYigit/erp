using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Domain.Entities.Approval
{
    public class ApprovalStep
    {
        public int Id { get; set; }
        public int StepNumber { get; set; }
        public string UserRoleId { get; set; }
        public int ApprovalScenarioId { get; set; }
        public ApprovalScenario ApprovalScenario { get; set; }
    }
}
