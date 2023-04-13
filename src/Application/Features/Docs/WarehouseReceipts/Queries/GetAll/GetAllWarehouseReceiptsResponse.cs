using ArdaManager.Domain.Entities.Corporation;
using ArdaManager.Domain.Entities.Human;
using ArdaManager.Domain.Entities.Inventory;
using ArdaManager.Domain.Entities.Misc;
using ArdaManager.Domain.Entities.Transactions.WarehouseDocs;
using ArdaManager.Domain.Enums.Doc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Application.Features.Docs.WarehouseReceipts.Queries.GetAll
{
    public class GetAllWarehouseReceiptsResponse
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
        public string WayBillNo { get; set; }
        public DateTime? WayBillDate { get; set; }


        public int WarehouseId { get; set; }
        public string WarehouseName { get; set; }
        public int? RelatedWarehouseId { get; set; }
        public string RelatedWarehouseName { get; set; }

        public string OwnerName { get; set; }
        public int? RelatedCompanyId { get; set; }
        public string RelatedCompanyName { get; set; }

        public string CreatedBy { get; set; }
        public int? PrevDocId { get; set; }
        public int? NextDocId { get; set; }


        public string PurchaseOrderDocNo { get; set; }
        public string PurchaseRequestDocNo { get; set; }
        public int? PurchaseRequestId { get; set; }
        public string WarehouseOfficerId { get; set; }
        public string WarehouseOfficerName { get; set; }

        public int? PurchaseOrderId { get; set; }


        public virtual Currency Currency { get; set; }
        public virtual BaseDocResponse PrevDoc { get; set; }
        public virtual BaseDocResponse NextDoc { get; set; }
        public virtual Company RelatedCompany { get; set; }

        public List<WarehouseReceiptRowsDto> WarehouseReceiptRowsDto { get; set; }

    }

    public class WarehouseReceiptRowsDto
    {
        public int Id { get; set; }
        public int RowNo { get; set; }
        public string Description { get; set; }
        public int WarehouseReceiptId { get; set; }
        public int ProductId { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public decimal Quantity { get; set; }
        public int MeasurementUnitId { get; set; }
        public string MeasurementUnitName { get; set; }
        public string MeasurementUnitCode { get; set; }


        /* PRICE PROPERTIES*/
        public decimal Price { get; set; } = decimal.Zero;
        public int CurrencyId { get; set; }
        public string CurrencyCode { get; set; }
        public string CurrencyName { get; set; }

        public decimal ExchangeRate { get; set; } = decimal.Zero;
        public int? TaxId { get; set; }
        public string TaxCode { get; set; }

        public decimal TaxAmount { get; set; } = decimal.Zero;

        public decimal DiscountPercentage { get; set; }

        public decimal DiscountAmount { get; set; } = decimal.Zero;

        public decimal TotalAmount { get; set; } = decimal.Zero;

        public virtual Currency Currency { get; set; }
        public virtual Tax Tax { get; set; }
        /* PRICE PROPERTIES*/

        public int? RackId { get; set; }
        public string RackCode { get; set; }
        public string RackName { get; set; }

        public int? PurchaseOrderRowId { get; set; }
        public string MeasurementUnitSystem { get; set; }
    }

    public class WarehouseReceiptRowComparer : IEqualityComparer<WarehouseReceiptRow>
    {
        public bool Equals(WarehouseReceiptRow x, WarehouseReceiptRow y)
        {
            if (x == null || y == null)
            {
                return false;
            }
            return x.Id == y.Id;
        }

        public int GetHashCode(WarehouseReceiptRow obj)
        {
            return obj.Id.GetHashCode();
        }
    }

}
