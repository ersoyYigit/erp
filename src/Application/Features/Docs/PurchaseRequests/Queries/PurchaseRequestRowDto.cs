using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Application.Features.Docs.PurchaseRequests.Queries
{
    public class PurchaseRequestRowDto
    {
        public int Id { get; set; }
        public decimal Quantity { get; set; }
        public string Description { get; set; }
        public int ProductId { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public int MeasurementUnitId { get; set; }
        public string MeasurementUnitCode { get; set; }
        public int RowNo { get; set; }
        public string MeasurementUnitName { get; set; }
        public string MeasurementUnitSystem { get; set; }
        public int PurchaseRequestId { get; set; }
    }
}
