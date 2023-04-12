using ArdaManager.Domain.Entities.Inventory;
using ArdaManager.Domain.Entities.Transactions.Purchase;
using ArdaManager.Domain.Enums.Doc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Domain.Entities.Transactions.WarehouseDocs
{
    [Table("WarehouseRequests")]
    public class WarehouseRequest : BaseDoc
    {
        public WarehouseRequest()
        {
            this.DocType = DocType.WarehouseRequest;
            WarehouseRequestRows = new HashSet<WarehouseRequestRow>();
        }


        public int WarehouseId { get; set; }
        public int RelatedWarehouseId { get; set; }


        public string WarehouseOfficerId { get; set; }
        public string WarehouseOfficerName { get; set; }



        public string RequesterId { get; set; }
        public string RequesterName { get; set; }
        public string RequesterDepartment { get; set; }
        
        public WarehouseReceiptType WarehouseReceiptType { get; set; }



        public virtual Warehouse Warehouse { get; set; }
        public virtual Warehouse RelatedWarehouse { get; set; }

        public virtual ICollection<WarehouseRequestRow> WarehouseRequestRows { get; set; }
    }
}
