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

namespace ArdaManager.Application.Features.Cities.Queries.GetAll
{

    public class GetAllCitiesQuery : IRequest<Result<List<GetAllCitiesResponse>>>
    {
        public GetAllCitiesQuery()
        {
        }
    }

    internal class GetAllCitiesCachedQueryHandler : IRequestHandler<GetAllCitiesQuery, Result<List<GetAllCitiesResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAppCache _cache;

        public GetAllCitiesCachedQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IAppCache cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<Result<List<GetAllCitiesResponse>>> Handle(GetAllCitiesQuery request, CancellationToken cancellationToken)
        {
            Func<Task<List<City>>> getAllCities = () => _unitOfWork.Repository<City>().GetAllAsync();
            var cityList = await _cache.GetOrAddAsync(ApplicationConstants.Cache.GetAllCitiesCacheKey, getAllCities);
            var mappedCities = _mapper.Map<List<GetAllCitiesResponse>>(cityList);
            return await Result<List<GetAllCitiesResponse>>.SuccessAsync(mappedCities);
        }
    }
}
