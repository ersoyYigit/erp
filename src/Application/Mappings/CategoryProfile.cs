using ArdaManager.Application.Features.Categories.Commands.AddEdit;
using ArdaManager.Application.Features.Categories.Queries.GetAll;
using ArdaManager.Application.Features.Categories.Queries.GetById;
using ArdaManager.Application.Features.Categories.Queries.GetCategoriesByType;
using ArdaManager.Domain.Entities.Misc;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Application.Mappings
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<AddEditCategoryCommand, Category>().ReverseMap();
            CreateMap<GetCategoryByIdResponse, Category>().ReverseMap();
            CreateMap<GetCategoriesByTypeResponse, Category>().ReverseMap();
            CreateMap<GetAllCategoriesResponse, Category>().ReverseMap();
        }
    }
}
