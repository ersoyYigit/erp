using ArdaManager.Domain.Enums.Doc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Application.Features.Docs
{
    public partial class BaseDocResponse
    {
        public string DocNo { get; set; }
        public DocType DocType { get; set; }
        public DocState DocState { get; set; }
        public Guid Guid { get; set; }
        public DateTime? DocDate { get; set; }
        public string Description { get; set; }
    }
}
