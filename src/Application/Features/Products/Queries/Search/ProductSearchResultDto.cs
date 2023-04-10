using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Application.Features.Products.Queries.Search
{
    public class ProductSearchResultDto
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string MeasurementUnitName { get; set; }
        public string MeasurementUnitSystem { get; set; }
        public int MeasurementUnitId { get; set; }
        public int? CategoryId { get; set; }
        public string CategoryName { get; set; }
        public decimal Quantity { get; set; }
        public string StockUnitCode { get; set; }
    }

}
