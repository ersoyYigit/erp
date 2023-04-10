using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArdaManager.Application.Features.Patterns.Queries.GetById;
using ArdaManager.Application.Features.ProductCategories.Commands.AddEdit;
using ArdaManager.Application.Features.ProductCategories.Queries.GetAll;
using ArdaManager.Domain.Entities.Catalog;
using ArdaManager.Application.Features.ProductCategories.Queries.GetById;

namespace ArdaManager.Application.Mappings
{
    public class ProductCategoryProfile : Profile
    {
        public ProductCategoryProfile()
        {
            CreateMap<AddEditProductCategoryCommand, ProductCategory>().ReverseMap();
            CreateMap<GetProductCategoryByIdResponse, ProductCategory>().ReverseMap();
            CreateMap<GetAllProductCategoriesResponse, ProductCategory>().ReverseMap();
        }
    }
}
