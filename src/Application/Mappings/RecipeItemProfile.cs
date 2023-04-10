using ArdaManager.Application.Features.RecipeItems.Commands.AddEdit;
using ArdaManager.Application.Features.RecipeItems.Queries.GetByOwnerProductId;
using ArdaManager.Domain.Entities.Catalog;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Application.Mappings
{
    public class RecipeItemProfile : Profile
    {
        public RecipeItemProfile()
        {
            CreateMap<AddEditRecipeItemCommand, RecipeItem>().ReverseMap();
            CreateMap<GetRecipeItemsByOwnerProductIdResponse, RecipeItem>().ReverseMap();
        }
    }
}
