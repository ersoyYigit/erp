﻿using ArdaManager.Domain.Enums;
using System;

namespace ArdaManager.Application.Features.Molds.Queries.GetAllPaged
{
    public class GetAllPagedMoldsResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public decimal Rate { get; set; }
        public string Pattern { get; set; }
        public string ImageDataURL { get; set; }
        public int? PatternId { get; set; }

        public string Category { get; set; }
        public int? CategoryId { get; set; }

        public int MeasurementUnitId { get; set; }
        public string MeasurementUnit { get; set; }

        public DateTime? LastProductionDate { get; set; }
        public ProductType Type { get; set; }
        public ProductKind Kind { get; set; }
        public TemplateState TemplateState { get; set; }
        public int? TemplateId { get; set; }
        public bool? BoolProperty1 { get; set; }
        public decimal Diameter { get; set; }
        public decimal DiameterTolerance { get; set; }
        public int? DiameterMUId { get; set; }
        public decimal Weight { get; set; }
        public decimal WeightTolerance { get; set; }
        public int? WeightMUId { get; set; }
        public decimal Height { get; set; }
        public decimal HeightTolerance { get; set; }
        public int? HeightMUId { get; set; }
        public decimal Length { get; set; }
        public decimal LengthTolerance { get; set; }
        public int? LengthMUId { get; set; }
        public decimal Width { get;  set; }
        public decimal WidthTolerance { get; set; }
        public int? WidthMUId { get; set; }



        public int? TemplatePatternId { get; set; }
        public DateTime? ProductionDate { get; set; }

        public bool IsNew { get; set; }

        public int? CompanyId { get; set; }
        public string SupplierCompany { get; set; }
        public string ModelOwner { get; set; }

        public string String1 { get; set; }
        public Guid Guid { get; set; }
    }
}