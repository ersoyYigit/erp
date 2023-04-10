using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Domain.Entities.Report.Warehouse
{
    public class WarehouseReport
    {
        public WarehouseReport()
        {

        }
        [Key]
        //public int Id { get; set; }
        public int WarehouseId { get; set; }
        public string WarehouseCode { get; set; }
        public string WarehouseName { get; set; }


        
        public int ProductId { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }

        
        public int? CategoryId { get; set; }
        public string CategoryName { get; set; }
        public decimal Quantity { get; set; }
    }
}
