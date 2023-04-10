using ArdaManager.Application.Features.ProductionLines.Commands.AddEdit;
using ArdaManager.Application.Features.ProductionLines.Queries.GetAll;
using ArdaManager.Application.Features.ProductionLines.Queries.GetById;
using ArdaManager.Domain.Entities.Inventory;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Application.Mappings
{
    public class ProductionLineProfile : Profile
    {
        public ProductionLineProfile()
        {
            CreateMap<AddEditProductionLineCommand, ProductionLine>().ReverseMap();
            CreateMap<GetProductionLineByIdResponse, ProductionLine>().ReverseMap();
            CreateMap<GetAllProductionLinesResponse, ProductionLine>().ReverseMap();
        }
    }
}
