using ArdaManager.Application.Features.Warehouses.Commands.AddEdit;
using ArdaManager.Application.Features.Warehouses.Queries.GetAll;
using ArdaManager.Application.Features.Warehouses.Queries.GetById;
using ArdaManager.Domain.Entities.Inventory;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Application.Mappings
{
    public class WarehouseProfile : Profile
    {
        public WarehouseProfile()
        {
            CreateMap<AddEditWarehouseCommand, Warehouse>().ReverseMap();
            CreateMap<GetWarehouseByIdResponse, Warehouse>().ReverseMap();
            CreateMap<GetAllWarehousesResponse, Warehouse>().ReverseMap();
        }
    }
}
