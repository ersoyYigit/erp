using ArdaManager.Application.Interfaces.Repositories;
using ArdaManager.Domain.Entities.Addressing;
using ArdaManager.Shared.Constants.Application;
using ArdaManager.Shared.Wrapper;
using AutoMapper;
using LazyCache;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ArdaManager.Application.Features.Countries.Queries.GetAll
{

    public class GetAllCountriesQuery : IRequest<Result<List<GetAllCountriesResponse>>>
    {
        public GetAllCountriesQuery()
        {
        }
    }

    internal class GetAllCountriesCachedQueryHandler : IRequestHandler<GetAllCountriesQuery, Result<List<GetAllCountriesResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAppCache _cache;

        public GetAllCountriesCachedQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IAppCache cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<Result<List<GetAllCountriesResponse>>> Handle(GetAllCountriesQuery request, CancellationToken cancellationToken)
        {
            Func<Task<List<Country>>> getAllCountries = () => _unitOfWork.Repository<Country>().GetAllAsync();
            var patternList = await _cache.GetOrAddAsync(ApplicationConstants.Cache.GetAllCountriesCacheKey, getAllCountries);
            var mappedCountries = _mapper.Map<List<GetAllCountriesResponse>>(patternList);
            return await Result<List<GetAllCountriesResponse>>.SuccessAsync(mappedCountries);
        }
    }
}
