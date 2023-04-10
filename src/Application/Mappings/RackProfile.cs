using ArdaManager.Application.Features.Racks.Commands.AddEdit;
using ArdaManager.Application.Features.Racks.Queries.GetAll;
using ArdaManager.Application.Features.Racks.Queries.GetById;
using ArdaManager.Domain.Entities.Inventory;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Application.Mappings
{
    public class RackProfile : Profile
    {
        public RackProfile()
        {
            CreateMap<AddEditRackCommand, Rack>().ReverseMap();
            CreateMap<GetRackByIdResponse, Rack>().ReverseMap();
            CreateMap<GetAllRacksResponse, Rack>().ReverseMap();
        }
    }
}
