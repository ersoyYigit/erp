using ArdaManager.Application.Features.Categories.Commands.AddEdit;
using ArdaManager.Application.Interfaces.Repositories;
using ArdaManager.Domain.Entities.Catalog;
using ArdaManager.Domain.Entities.Misc;
using ArdaManager.Shared.Constants.Application;
using ArdaManager.Shared.Wrapper;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ArdaManager.Application.Features.RecipeItems.Commands.AddEdit
{
    public partial class AddEditRecipeItemCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        public int OwnerProductId { get; set; }
        public int RecipeProductId { get; set; }
        public int WarehouseId { get; set; }
        public decimal Quantity { get; set; }
        public string Description { get; set; }
    }


    internal class AddEditRecipeItemCommandHandler : IRequestHandler<AddEditRecipeItemCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<AddEditRecipeItemCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public AddEditRecipeItemCommandHandler(IMapper mapper, IStringLocalizer<AddEditRecipeItemCommandHandler> localizer, IUnitOfWork<int> unitOfWork)
        {
            _mapper = mapper;
            _localizer = localizer;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<int>> Handle(AddEditRecipeItemCommand command, CancellationToken cancellationToken)
        {
            if (command.Id == 0)
            {
                var recipeItem = _mapper.Map<RecipeItem>(command);
                try
                {
                    await _unitOfWork.Repository<RecipeItem>().AddAsync(recipeItem);
                    await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllRecipeItemssByTypeCacheKey);
                }
                catch (Exception ex)
                {
                    var x = ex.Message;
                    throw;
                }
                return await Result<int>.SuccessAsync(recipeItem.Id, _localizer["RecipeItem Saved"]);
            }
            else
            {
                try
                {
                    var recipeItem = await _unitOfWork.Repository<RecipeItem>().GetByIdAsync(command.Id);
                    if (recipeItem != null)
                    {
                        recipeItem.OwnerProductId = (command.OwnerProductId == 0) ? recipeItem.OwnerProductId : command.OwnerProductId;
                        recipeItem.RecipeProductId = (command.RecipeProductId == 0) ? recipeItem.RecipeProductId : command.RecipeProductId;
                        recipeItem.WarehouseId = (command.WarehouseId == 0) ? recipeItem.WarehouseId : command.WarehouseId;
                        recipeItem.Description = command.Description ?? recipeItem.Description;
                        recipeItem.Quantity = command.Quantity;
                        await _unitOfWork.Repository<RecipeItem>().UpdateAsync(recipeItem);
                        await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllRecipeItemssByTypeCacheKey);
                        return await Result<int>.SuccessAsync(recipeItem.Id, _localizer["RecipeItem Updated"]);
                    }
                    else
                    {
                        return await Result<int>.FailAsync(_localizer["RecipeItem Not Found!"]);
                    }
                }
                catch (Exception ex)
                {
                    var messsage = ex.Message;
                    throw;
                }
                
            }
        }

    }

}
