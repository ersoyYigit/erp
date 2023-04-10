using ArdaManager.Application.Features.Docs.WarehouseReceipts.Queries.GetAll;
using ArdaManager.Domain.Entities.Transactions.WarehouseDocs;
using ArdaManager.Domain.Enums.Doc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Application.Features.Docs.WarehouseReceipts.Queries.GetById
{
    public class GetWarehouseReceiptByIdResponse
    {
        public int Id { get; set; }
        public string DocNo { get; set; }
        public DocType DocType { get; set; }
        public DateTime? DocDate { get; set; }

        public WarehouseReceiptType WarehouseReceipType { get; set; }
        public string WayBillNo { get; set; }
        public DateTime? WayBillDate { get; set; }


        public int WarehouseId { get; set; }
        public string WarehouseName { get; set; }
        public int? RelatedWarehouseId { get; set; }
        public string RelatedWarehouseName { get; set; }

        public int? WarehouseOfficerId { get; set; }
        public string WarehouseOfficerGuid { get; set; }
        public string WarehouseOfficerName { get; set; }
        public string OwnerName { get; set; }
        public int? RelatedCompanyId { get; set; }
        public string RelatedCompanyName { get; set; }

        public List<WarehouseReceiptRowsDto> WarehouseReceiptRowsDto { get; set; }
    }
}
