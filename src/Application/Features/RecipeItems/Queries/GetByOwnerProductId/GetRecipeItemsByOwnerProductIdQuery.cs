using ArdaManager.Application.Features.Categories.Queries.GetCategoriesByType;
using ArdaManager.Application.Features.Warehouses.Queries.GetAll;
using ArdaManager.Application.Interfaces.Repositories;
using ArdaManager.Domain.Entities.Catalog;
using ArdaManager.Domain.Entities.Inventory;
using ArdaManager.Domain.Entities.Misc;
using ArdaManager.Shared.Constants.Application;
using ArdaManager.Shared.Wrapper;
using AutoFilterer.Types;
using AutoMapper;
using LazyCache;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static ArdaManager.Shared.Constants.Application.ApplicationConstants;

namespace ArdaManager.Application.Features.RecipeItems.Queries.GetByOwnerProductId
{
    public class GetRecipeItemsByOwnerProductIdQuery : PaginationFilterBase, IRequest<Result<List<GetRecipeItemsByOwnerProductIdResponse>>>
    {
        public GetRecipeItemsByOwnerProductIdQuery()
        {
        }
        public int OwnerProductId { get; set; }
    }

    internal class GetRecipeItemsByOwnerProductIdQueryHandler : IRequestHandler<GetRecipeItemsByOwnerProductIdQuery, Result<List<GetRecipeItemsByOwnerProductIdResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IRecipeItemRepository _recipeItemRepository;
        private readonly IMapper _mapper;
        private readonly IVappRepository _repo;
        private readonly IAppCache _cache;

        public GetRecipeItemsByOwnerProductIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IRecipeItemRepository recipeItemRepository, IVappRepository repo, IAppCache cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _recipeItemRepository = recipeItemRepository;
            _repo = repo;
            _cache = cache;
        }

        public async Task<Result<List<GetRecipeItemsByOwnerProductIdResponse>>> Handle(GetRecipeItemsByOwnerProductIdQuery request, CancellationToken cancellationToken)
        {


            //Func<Task<List<RecipeItem>>> getAllRecipeItems = () => _recipeItemRepository.GetRecipeItemsByOwnerProductId(request.OwnerProductId);

            Expression<Func<RecipeItem, GetRecipeItemsByOwnerProductIdResponse>> expression = e => new GetRecipeItemsByOwnerProductIdResponse
            {
                Id = e.Id,
                OwnerProductCode = e.OwnerProduct.Code,
                OwnerProductDisplay = e.OwnerProduct.Code + " - "  + e.OwnerProduct.Name,
                OwnerProductId = e.OwnerProductId,
                OwnerProductName = e.OwnerProduct.Name,
                Quantity = e.Quantity,
                RecipeProductCode = e.RecipeProduct.Code,
                RecipeProductDisplay = e.RecipeProduct.Code + " - " + e.RecipeProduct.Name,
                RecipeProductId = e.RecipeProductId,    
                RecipeProductName = e.RecipeProduct.Name,
                RecipeItemProductType = e.RecipeProduct.Type,
                WarehouseId = e.WarehouseId,
                WarehouseName = e.Warehouse.Name,
                MeasuremetUnitId = e.RecipeProduct.MeasurementUnitId,
                MeasuremetUnitName = e.RecipeProduct.MeasurementUnit.Code,
                Description = e.Description

            };

            var data = await _unitOfWork.Repository<RecipeItem>().Entities
                   //.Include(x => x.Racks)
                   .Select(expression)
                   
                   .ToListAsync();

            //var recipeItemList = await _cache.GetOrAddAsync(ApplicationConstants.Cache.GetAllRecipeItemssByTypeCacheKey + request.OwnerProductId.ToString(), data);
            var mappedRecipeItems = _mapper.Map<List<GetRecipeItemsByOwnerProductIdResponse>>(data);
            return await Result<List<GetRecipeItemsByOwnerProductIdResponse>>.SuccessAsync(mappedRecipeItems);

        }
    }
}
