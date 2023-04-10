using AutoMapper;
using ArdaManager.Domain.Entities.Catalog;
using ArdaManager.Application.Features.Templates.Commands.AddEdit;
using ArdaManager.Application.Features.Molds.Commands.AddEdit;
using ArdaManager.Application.Features.Molds.Queries.GetAllPaged;

namespace ArdaManager.Application.Mappings
{
    public class MoldProfile : Profile
    {
        public MoldProfile()
        {
            CreateMap<AddEditMoldCommand, Mold>().ReverseMap();
            CreateMap<GetAllPagedMoldsQuery, Mold>().ReverseMap();
        }
    }
}