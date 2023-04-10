using ArdaManager.Domain.Enums.Doc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Application.Responses.Approval
{
    public class ApprovalScenarioResponse
    {
        public int Id { get; set; }
        public DocType DocType { get; set; }
        public string Description { get; set; }
        public ICollection<ApprovalStepResponse> ApprovalSteps { get; set; }
    }
}
