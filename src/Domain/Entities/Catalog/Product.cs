using ArdaManager.Domain.Contracts;
using ArdaManager.Domain.Entities.Addressing;
using ArdaManager.Domain.Entities.Misc;
using ArdaManager.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArdaManager.Domain.Entities.Catalog
{
    [Table("Products")]
    public class Product : BaseEntity
    {

        [Column(Order = 1)]
        public ProductType Type { get; set; }
        [Column(TypeName = "text")]
        public string ImageDataURL { get; set; }


        public string Description { get; set; }
        public int? PatternId { get; set; }
        public int? CategoryId { get; set; }
        public int MeasurementUnitId { get; set; }


        /*DIMENSIONS*/
        public decimal Weight { get; set; }
        public decimal WeightTolerance { get; set; }
        public int? WeightMUId { get; set; }
        public decimal Diameter { get; set; }
        public decimal DiameterTolerance { get; set; }
        public int? DiameterMUId { get; set; }
        public decimal Height { get; set; }
        public decimal HeightTolerance { get; set; }
        public int? HeightMUId { get; set; }
        public decimal Length { get; set; }
        public decimal LengthTolerance { get; set; }
        public int? LengthMUId { get; set; }
        public decimal Width { get; set; }
        public decimal WidthTolerance { get; set; }
        public int? WidthMUId { get; set; }
        /*DIMENSIONS*/

        public DateTime? LastProductionDate { get; set; }
        
        public ProductKind Kind { get; set; }
        public TemplateState TemplateState { get; set; }
        public int? TemplateId { get; set; }
        public bool? BoolProperty1 { get; set; }

        public string string1 { get; set; }
        public string string2 { get; set; }
        public string string3 { get; set; }
        public string string4 { get; set; }
        public string string5 { get; set; }
        public string string6 { get; set; }

        public int int1 { get; set; }
        public int int2 { get; set; }
        public int int3 { get; set; }

        public virtual Pattern Pattern { get; set; }
        public virtual Category Category { get; set; }
        public virtual Product Template { get; set; }
        public virtual MeasurementUnit MeasurementUnit { get; set; }
        
    }
}