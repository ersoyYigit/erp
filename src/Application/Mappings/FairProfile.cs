using ArdaManager.Application.Features.Patterns.Commands.AddEdit;
using ArdaManager.Application.Features.Fairs.Commands.AddEdit;
using ArdaManager.Application.Features.Fairs.Queries.GetAll;
using ArdaManager.Application.Features.Fairs.Queries.GetById;
using ArdaManager.Domain.Entities.Corporation;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Application.Mappings
{
    public class FairProfile : Profile
    {
        public FairProfile()
        {
            CreateMap<AddEditFairCommand, Fair>().ReverseMap();
            CreateMap<GetFairByIdResponse, Fair>().ReverseMap();
            CreateMap<GetAllFairsResponse, Fair>().ReverseMap();
        }
    
    }
}
