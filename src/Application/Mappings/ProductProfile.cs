using AutoMapper;
using ArdaManager.Application.Features.Products.Commands.AddEdit;
using ArdaManager.Domain.Entities.Catalog;

namespace ArdaManager.Application.Mappings
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<AddEditProductCommand, Product>().ReverseMap();
        }
    }
}