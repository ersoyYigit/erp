using ArdaManager.Domain.Contracts;
using ArdaManager.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Domain.Entities
{
    public class BaseEntity : AuditableEntity<int>
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public BaseEntityType BaseEntityType { get; set; } = BaseEntityType.Empty;
        public Guid Guid { get; set; } = Guid.NewGuid();

    }
}
