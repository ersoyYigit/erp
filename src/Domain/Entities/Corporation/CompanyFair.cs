using ArdaManager.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Domain.Entities.Corporation
{
    public class CompanyFair: AuditableEntity<int>
    {
        public int CompanyId { get; set; }
        public int FairId { get; set; }
        public virtual Company Company { get; set; }
        public virtual Fair Fair { get; set; }
    }
}
