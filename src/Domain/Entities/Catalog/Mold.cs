using ArdaManager.Domain.Contracts;
using ArdaManager.Domain.Entities.Addressing;
using ArdaManager.Domain.Entities.Corporation;
using ArdaManager.Domain.Entities.Misc;
using ArdaManager.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArdaManager.Domain.Entities.Catalog
{
    [Table("Molds")]
    public class Mold : Product
    {

        public Mold()
        {
            this.BaseEntityType = BaseEntityType.Mold;
        }

        public bool IsNew { get; set; } = true;
        public string ModelOwner { get; set; }

        public int? UsageYear { get; set; }
        public int? CompanyId { get; set; }

        public DateTime? ProductionDate { get; set; }
        public int? TemplatePatternId { get; set; }
        public virtual Pattern TemplatePattern { get; set; }
        public virtual Company Company { get; set; }



    }
}