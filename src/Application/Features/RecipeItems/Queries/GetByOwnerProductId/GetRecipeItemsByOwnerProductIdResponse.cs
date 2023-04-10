using ArdaManager.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Application.Features.RecipeItems.Queries.GetByOwnerProductId
{
    public class GetRecipeItemsByOwnerProductIdResponse
    {
        public int Id { get; set; }
        public int OwnerProductId { get; set; }
        public string OwnerProductName { get; set; }
        public string OwnerProductCode { get; set; }
        public string OwnerProductDisplay { get; set; }
        public int RecipeProductId { get; set; }
        public string RecipeProductName { get; set; }
        public string RecipeProductCode { get; set; }
        public string RecipeProductDisplay { get; set; }
        public ProductType RecipeItemProductType { get; set; }
        public int WarehouseId { get; set; }
        public string WarehouseName { get; set; }
        public decimal Quantity { get; set; }
        public int MeasuremetUnitId { get; set; }
        public string MeasuremetUnitName { get; set; }
        public string Description { get; set; }

    }
}
