using ArdaManager.Application.Interfaces.Repositories;
using ArdaManager.Domain.Entities.Misc;
using ArdaManager.Domain.Enums;
using ArdaManager.Shared.Constants.Application;
using ArdaManager.Shared.Wrapper;
using AutoFilterer.Types;
using AutoMapper;
using LazyCache;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ArdaManager.Application.Features.Categories.Queries.GetCategoriesByType
{
    public class GetCategoriesByTypeQuery : PaginationFilterBase, IRequest<Result<List<GetCategoriesByTypeResponse>>>
    {
        public GetCategoriesByTypeQuery()
        {
        }
        public int Type { get; set; }
    }

    internal class GetCategoriesByTypeQueryHandler : IRequestHandler<GetCategoriesByTypeQuery, Result<List<GetCategoriesByTypeResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly IVappRepository _repo;
        private readonly IAppCache _cache;

        public GetCategoriesByTypeQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, ICategoryRepository categoryRepository, IVappRepository repo, IAppCache cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _categoryRepository = categoryRepository;
            _repo = repo;
            _cache = cache;
        }


        public async Task<Result<List<GetCategoriesByTypeResponse>>> Handle(GetCategoriesByTypeQuery request, CancellationToken cancellationToken)
        {


            Func<Task<List<Category>>> getAllCategories = () => _categoryRepository.GetCategoriesByType(request.Type);

            var categoryList = await _cache.GetOrAddAsync(ApplicationConstants.Cache.GetAllCategoriesByTypeCacheKey + request.Type.ToString(), getAllCategories);
            var mappedCategories = _mapper.Map<List<GetCategoriesByTypeResponse>>(categoryList);
            return await Result<List<GetCategoriesByTypeResponse>>.SuccessAsync(mappedCategories);

        }
    }
}
