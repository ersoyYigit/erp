using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArdaManager.Domain.Entities.Corporation;
using ArdaManager.Domain.Entities.Human;
using ArdaManager.Domain.Entities.Inventory;
using ArdaManager.Domain.Entities.Transactions.Purchase;
using ArdaManager.Domain.Enums.Doc;

namespace ArdaManager.Domain.Entities.Transactions.WarehouseDocs
{
    [Table("WarehouseReceipts")]
    public class WarehouseReceipt : BaseDoc
    {
        public WarehouseReceipt()
        {
            //this.DocType = DocType.Warehouse;
        }

        public WarehouseReceiptType WarehouseReceiptType { get; set; }
        public string WayBillNo { get; set; }
        public DateTime? WayBillDate { get; set; }


        public int WarehouseId { get; set; }
        public int? RelatedWarehouseId { get; set; }

        public string WarehouseOfficerId { get; set; }
        public string WarehouseOfficerName { get; set; }
        public int? RelatedCompanyId { get; set; }


        public string RequesterId { get; set; }
        public string RequesterName { get; set; }
        public string RequesterDepartment { get; set; }

        public int? PurchaseOrderId { get; set; }
        public string PurchaseOrderDocNo { get; set; }
        public string PurchaseRequestDocNo { get; set; }
        public int? PurchaseRequestId { get; set; }

        public virtual PurchaseRequest PurchaseRequest { get; set; }
        public virtual PurchaseOrder PurchaseOrder { get; set; }
        public virtual Warehouse Warehouse { get; set; }
        public virtual Warehouse RelatedWarehouse { get; set; }
        public virtual Company RelatedCompany { get; set; }
        public virtual ICollection<WarehouseReceiptRow> WarehouseReceiptRows { get; set; }
    }
}
