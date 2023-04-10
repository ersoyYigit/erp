using ArdaManager.Application.Features.Categories.Commands.Delete;
using ArdaManager.Application.Interfaces.Repositories;
using ArdaManager.Domain.Entities.Catalog;
using ArdaManager.Domain.Entities.Misc;
using ArdaManager.Shared.Constants.Application;
using ArdaManager.Shared.Wrapper;
using MediatR;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ArdaManager.Application.Features.RecipeItems.Commands.Delete
{
    public class DeleteRecipeItemCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }


    internal class DeleteRecipeItemCommandHandler : IRequestHandler<DeleteRecipeItemCommand, Result<int>>
    {

        private readonly IStringLocalizer<DeleteRecipeItemCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public DeleteRecipeItemCommandHandler(IUnitOfWork<int> unitOfWork, IStringLocalizer<DeleteRecipeItemCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(DeleteRecipeItemCommand command, CancellationToken cancellationToken)
        {
            var recipeItem = await _unitOfWork.Repository<RecipeItem>().GetByIdAsync(command.Id);
            if (recipeItem != null)
            {
                await _unitOfWork.Repository<RecipeItem>().DeleteAsync(recipeItem);
                await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllCategoriesCacheKey);
                return await Result<int>.SuccessAsync(recipeItem.Id, _localizer["RecipeItem Deleted"]);
            }
            else
            {
                return await Result<int>.FailAsync(_localizer["RecipeItem Not Found!"]);
            }

        }

    }
}