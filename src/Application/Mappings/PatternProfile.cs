using AutoMapper;
using ArdaManager.Application.Features.Patterns.Commands.AddEdit;
using ArdaManager.Application.Features.Patterns.Queries.GetAll;
using ArdaManager.Application.Features.Patterns.Queries.GetById;
using ArdaManager.Domain.Entities.Catalog;

namespace ArdaManager.Application.Mappings
{
    public class PatternProfile : Profile
    {
        public PatternProfile()
        {
            CreateMap<AddEditPatternCommand, Pattern>().ReverseMap();
            CreateMap<GetPatternByIdResponse, Pattern>().ReverseMap();
            CreateMap<GetAllPatternsResponse, Pattern>().ReverseMap();
        }
    }
}