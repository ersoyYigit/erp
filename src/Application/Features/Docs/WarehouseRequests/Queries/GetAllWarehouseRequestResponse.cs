using ArdaManager.Domain.Entities.Corporation;
using ArdaManager.Domain.Entities.Misc;
using ArdaManager.Domain.Entities.Transactions.WarehouseDocs;
using ArdaManager.Domain.Enums.Doc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Application.Features.Docs.WarehouseRequests.Queries
{
    public class WarehouseRequestResponse
    {
        public int Id { get; set; }
        public string DocNo { get; set; }
        public DocType DocType { get; set; }
        public DocState DocState { get; set; }
        public DateTime? DocDate { get; set; }
        public string Description { get; set; }

        public string RequesterName { get; set; }
        public string RequesterId { get; set; }
        public string RequesterDepartment { get; set; }
        public WarehouseReceiptType WarehouseReceiptType { get; set; }


        public int WarehouseId { get; set; }
        public string WarehouseName { get; set; }
        public int RelatedWarehouseId { get; set; }
        public string RelatedWarehouseName { get; set; }


        public string CreatedBy { get; set; }
        public int? PrevDocId { get; set; }
        public int? NextDocId { get; set; }


        public virtual BaseDocResponse PrevDoc { get; set; }
        public virtual BaseDocResponse NextDoc { get; set; }

        public List<WarehouseRequestRowResponse> WarehouseRequestRowsResponse { get; set; }

    }

    public class WarehouseRequestRowResponse
    {
        public int Id { get; set; }
        public int RowNo { get; set; }
        public string Description { get; set; }
        public int WarehouseRequestId { get; set; }
        public int ProductId { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public decimal Quantity { get; set; }
        public int MeasurementUnitId { get; set; }
        public string MeasurementUnitName { get; set; }
        public string MeasurementUnitCode { get; set; }
        public string MeasurementUnitSystem { get; set; }



        public int? RackId { get; set; }
        public string RackCode { get; set; }
        public string RackName { get; set; }


        public virtual WarehouseRequest WarehouseRequest { get; set; }

    }

    public class WarehouseRequestRowComparer : IEqualityComparer<WarehouseRequestRow>
    {
        public bool Equals(WarehouseRequestRow x, WarehouseRequestRow y)
        {
            if (x == null || y == null)
            {
                return false;
            }
            return x.Id == y.Id;
        }

        public int GetHashCode(WarehouseRequestRow obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}
