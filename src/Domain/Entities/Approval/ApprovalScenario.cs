using ArdaManager.Domain.Enums.Doc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Domain.Entities.Approval
{
    public class ApprovalScenario
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DocType DocType { get; set; }
        public ICollection<ApprovalStep> ApprovalSteps { get; set; }
    }

}
