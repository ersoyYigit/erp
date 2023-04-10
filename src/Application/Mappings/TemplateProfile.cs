using AutoMapper;
using ArdaManager.Domain.Entities.Catalog;
using ArdaManager.Application.Features.Templates.Commands.AddEdit;

namespace ArdaManager.Application.Mappings
{
    public class TemplateProfile : Profile
    {
        public TemplateProfile()
        {
            CreateMap<AddEditTemplateCommand, Template>().ReverseMap();
        }
    }
}