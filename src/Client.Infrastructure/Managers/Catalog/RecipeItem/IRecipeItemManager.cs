using ArdaManager.Application.Features.Categories.Commands.AddEdit;
using ArdaManager.Application.Features.Categories.Queries.GetById;
using ArdaManager.Application.Features.RecipeItems.Commands.AddEdit;
using ArdaManager.Application.Features.RecipeItems.Queries.GetByOwnerProductId;
using ArdaManager.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Client.Infrastructure.Managers.Catalog.RecipeItem
{
    public interface IRecipeItemManager : IManager
    {
        Task<IResult<List<GetRecipeItemsByOwnerProductIdResponse>>> GetByOwnerProductIdAsync(GetRecipeItemsByOwnerProductIdQuery query);

        Task<IResult<int>> SaveAsync(AddEditRecipeItemCommand request);

        Task<IResult<int>> DeleteAsync(int id);
    }
}
