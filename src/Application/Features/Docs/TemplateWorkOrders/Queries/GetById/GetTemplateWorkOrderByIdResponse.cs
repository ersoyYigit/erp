using ArdaManager.Domain.Enums.Doc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Application.Features.Docs.TemplateWorkOrders.Queries.GetById
{
    public class GetTemplateWorkOrderByIdResponse
    {
        public int Id { get; set; }
        public string DocNo { get; set; }
        public DocType DocType { get; set; }
        public TemplateWorkOrderState TemplateWorkOrderState { get; set; }
        public DateTime? DocDate { get; set; }

        public string OwnerUserId { get; set; }
        public string OwnerUserName { get; set; }
        public string ResponseUserId { get; set; }
        public string ResponseUserName { get; set; }
        public TemplateWorkOrderType TemplateWorkOrderType { get; set; }


        public DateTime? WorkStartDate { get; set; }
        public DateTime? PlannedWorkEndDate { get; set; }
        public DateTime? PlannedWorkStartDate { get; set; }
        public DateTime? WorkEndDate { get; set; }


        public int? ConsumeProductId { get; set; }
        public string ConsumeProductCode { get; set; }
        public string ConsumeProductName { get; set; }
        public int? ProductionLineId { get; set; }
        public string ProductionLineCode { get; set; }
        public string ProductionLineName { get; set; }
        public int TemplateId { get; set; }
        public string TemplateCode { get; set; }
        public string TemplateName { get; set; }
    }
}
