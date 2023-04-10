using ArdaManager.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Domain.Entities.Inventory
{
    [Table("ProductionLines")]
    public class ProductionLine: BaseEntity
    {
        public ProductionLine()
        {
            this.BaseEntityType = Enums.BaseEntityType.ProductionLine;
        }
        public string Description { get; set; }


    }
}
