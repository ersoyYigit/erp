using ArdaManager.Domain.Entities.Catalog;
using ArdaManager.Domain.Entities.Inventory;
using ArdaManager.Domain.Enums.Doc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Domain.Entities.Transactions
{
    [Table("TemplateWorkOrders")]
    public class TemplateWorkOrder : BaseDoc
    {
        public TemplateWorkOrder()
        {
            this.DocType = DocType.TemplateWorkOrder;
        }


        public string OwnerUserId { get; set; }
        public string OwnerUserName { get; set; }
        public string ResponseUserId { get; set; }
        public string ResponseUserName { get; set; }
        public TemplateWorkOrderType TemplateWorkOrderType { get; set; }
        public TemplateWorkOrderState TemplateWorkOrderState { get; set; }


        public DateTime? PlannedWorkEndDate { get; set; }
        public DateTime? PlannedWorkStartDate { get; set; }
        public DateTime? WorkStartDate { get; set; }
        public DateTime? WorkEndDate { get; set; }





        public int TemplateId { get; set; }
        public int? ConsumeProductId { get; set; }
        public int? ProductionLineId { get; set; }
        
        public Product ConsumeProduct { get; set; }
        public Template Template { get; set; }
        public virtual ProductionLine ProductionLine { get; set; }
        


        public string Description { get; set; }
        public int? PatternId { get; set; }
    }
}
