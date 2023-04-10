using ArdaManager.Application.Interfaces.Repositories;
using ArdaManager.Domain.Entities.Misc;
using ArdaManager.Shared.Constants.Application;
using ArdaManager.Shared.Wrapper;
using AutoMapper;
using LazyCache;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ArdaManager.Application.Features.Categories.Queries.GetAll
{
    public class GetAllCategoriesQuery : IRequest<Result<List<GetAllCategoriesResponse>>>
    {
        public GetAllCategoriesQuery()
        {
        }
    }

    internal class GetAllCategoriesCachedQueryHandler : IRequestHandler<GetAllCategoriesQuery, Result<List<GetAllCategoriesResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAppCache _cache;

        public GetAllCategoriesCachedQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IAppCache cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<Result<List<GetAllCategoriesResponse>>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
        {
            Func<Task<List<Category>>> getAllCategories = () => _unitOfWork.Repository<Category>().GetAllAsync();
            var catgegoriesList = await _cache.GetOrAddAsync(ApplicationConstants.Cache.GetAllCategoriesCacheKey, getAllCategories);
            var mappedCategories = _mapper.Map<List<GetAllCategoriesResponse>>(catgegoriesList);
            return await Result<List<GetAllCategoriesResponse>>.SuccessAsync(mappedCategories);
        }
    }
}
