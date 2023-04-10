using ArdaManager.Application.Features.Categories.Commands.AddEdit;
using ArdaManager.Application.Features.Categories.Queries.GetAll;
using ArdaManager.Application.Features.Categories.Queries.GetById;
using ArdaManager.Application.Features.Categories.Queries.GetCategoriesByType;
using ArdaManager.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Client.Infrastructure.Managers.Misc.Category
{
    public interface ICategoryManager : IManager
    {
        Task<IResult<List<GetAllCategoriesResponse>>> GetAllAsync();
        Task<IResult<GetCategoryByIdResponse>> GetByIdAsync(int id);
        Task<IResult<List<GetCategoriesByTypeResponse>>> GetByTypeAsync(GetCategoriesByTypeQuery query);
        

        Task<IResult<int>> SaveAsync(AddEditCategoryCommand request);

        Task<IResult<int>> DeleteAsync(int id);

        
        
    }
}
