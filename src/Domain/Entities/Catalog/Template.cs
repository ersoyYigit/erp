using ArdaManager.Domain.Contracts;
using ArdaManager.Domain.Entities.Addressing;
using ArdaManager.Domain.Entities.Misc;
using ArdaManager.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArdaManager.Domain.Entities.Catalog
{
    [Table("Templates")]
    public class Template : Product
    {

        public Template()
        {
            this.BaseEntityType = BaseEntityType.Template;
        }
        public int? TemplatePatternId { get; set; }
        public DateTime? ProductionDate { get; set; }
        public TemplateKind TemplateKind { get; set; }
        public TemplateState State { get; set; }
        public bool? IsAlabastr { get; set; }

        /// <summary>
        /// Template sample 
        /// </summary>
        /// 

        public DateTime? RevisionDate { get; set; }
        public string RevisionRequester { get; set; }
        public string RevisionRequesterDepartment { get; set; }

        public DateTime? FaultDate { get; set; }
        public DateTime? FixDate { get; set; }
        public string FaultCause { get; set; }
        public string FaultFixer { get; set; }

        public string CancelCause { get; set; }
        public string Canceller { get; set; }
        public DateTime? CancelationDate { get; set; }



        public virtual Pattern TemplatePattern { get; set; }
        
    }
}