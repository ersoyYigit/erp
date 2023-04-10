using ArdaManager.Domain.Enums.Doc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Application.Requests.Approval
{
    public class UpdateDocStateRequest
    {
        public int BaseDocId { get; set; }
        public DocState NewDocState { get; set; }
    }
}
