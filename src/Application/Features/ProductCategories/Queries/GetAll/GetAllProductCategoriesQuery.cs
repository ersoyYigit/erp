using AutoMapper;
using LazyCache;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ArdaManager.Application.Interfaces.Repositories;
using ArdaManager.Domain.Entities.Catalog;
using ArdaManager.Shared.Constants.Application;
using ArdaManager.Shared.Wrapper;

namespace ArdaManager.Application.Features.ProductCategories.Queries.GetAll
{
    public class GetAllProductCategoriesQuery : IRequest<Result<List<GetAllProductCategoriesResponse>>>
    {
        public GetAllProductCategoriesQuery()
        {
        }
    }

    internal class GetAllProductCategoriesCachedQueryHandler : IRequestHandler<GetAllProductCategoriesQuery, Result<List<GetAllProductCategoriesResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAppCache _cache;

        public GetAllProductCategoriesCachedQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IAppCache cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<Result<List<GetAllProductCategoriesResponse>>> Handle(GetAllProductCategoriesQuery request, CancellationToken cancellationToken)
        {
            Func<Task<List<ProductCategory>>> getAllProductCategories = () => _unitOfWork.Repository<ProductCategory>().GetAllAsync();
            var productCategoryList = await _cache.GetOrAddAsync(ApplicationConstants.Cache.GetAllProductCategoriesCacheKey, getAllProductCategories);
            var mappedProductCategories = _mapper.Map<List<GetAllProductCategoriesResponse>>(productCategoryList);
            return await Result<List<GetAllProductCategoriesResponse>>.SuccessAsync(mappedProductCategories);
        }
    }
}
