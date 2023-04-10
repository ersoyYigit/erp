using ArdaManager.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Domain.Entities.Catalog
{
    public class MeasurementUnit : AuditableEntity<int>
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string System { get; set; }

        [Column(TypeName = "decimal(18,8)")]
        public decimal Multipler { get; set; }
        public int? InclusiveId { get; set; }
    }
}
