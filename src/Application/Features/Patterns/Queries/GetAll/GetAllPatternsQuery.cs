using AutoMapper;
using ArdaManager.Application.Interfaces.Repositories;
using ArdaManager.Domain.Entities.Catalog;
using ArdaManager.Shared.Constants.Application;
using ArdaManager.Shared.Wrapper;
using LazyCache;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ArdaManager.Application.Features.Patterns.Queries.GetAll;

namespace ArdaManager.Application.Features.Patterns.Queries.GetAll
{
    public class GetAllPatternsQuery : IRequest<Result<List<GetAllPatternsResponse>>>
    {
        public GetAllPatternsQuery()
        {
        }
    }

    internal class GetAllPatternsCachedQueryHandler : IRequestHandler<GetAllPatternsQuery, Result<List<GetAllPatternsResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAppCache _cache;

        public GetAllPatternsCachedQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IAppCache cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<Result<List<GetAllPatternsResponse>>> Handle(GetAllPatternsQuery request, CancellationToken cancellationToken)
        {
            Func<Task<List<Pattern>>> getAllPatterns = () => _unitOfWork.Repository<Pattern>().GetAllAsync();
            var patternList = await _cache.GetOrAddAsync(ApplicationConstants.Cache.GetAllPatternsCacheKey, getAllPatterns);
            var mappedPatterns = _mapper.Map<List<GetAllPatternsResponse>>(patternList);
            return await Result<List<GetAllPatternsResponse>>.SuccessAsync(mappedPatterns);
        }
    }
}