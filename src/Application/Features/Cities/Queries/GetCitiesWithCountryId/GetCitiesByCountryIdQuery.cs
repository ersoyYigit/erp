using ArdaManager.Application.Features.Patterns.Queries.GetById;
using ArdaManager.Application.Interfaces.Repositories;
using ArdaManager.Domain.Entities.Addressing;
using ArdaManager.Shared.Constants.Application;
using ArdaManager.Shared.Wrapper;
using AutoFilterer.Types;
using AutoMapper;
using LazyCache;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ArdaManager.Application.Features.Cities.Queries.GetCitiesWithCountryId
{
    
    public class GetCitiesByCountryIdQuery : PaginationFilterBase , IRequest<Result<List<GetCitiesByCountryIdResponse>>>
    {
        public GetCitiesByCountryIdQuery()
        {
        }
        public int CountryId { get; set; }
    }

    internal class GetCitiesByCountryIdQueryHandler : IRequestHandler<GetCitiesByCountryIdQuery, Result<List<GetCitiesByCountryIdResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly ICityRepository _cityRepository;
        private readonly IMapper _mapper;
        private readonly IVappRepository _repo;
        private readonly IAppCache _cache;

        public GetCitiesByCountryIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, ICityRepository cityRepository, IVappRepository repo, IAppCache cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cityRepository = cityRepository;
            _repo = repo;
            _cache = cache;
        }


        public async Task<Result<List<GetCitiesByCountryIdResponse>>> Handle(GetCitiesByCountryIdQuery request, CancellationToken cancellationToken)
        {


            Func<Task<List<City>>> getAllCities = () => _cityRepository.GetCitiesByCountryId(request.CountryId);

            //Func<Task<List<City>>> getAllCities = () => _unitOfWork.Repository<City>().GetAllAsync();
            
            var cityList = await _cache.GetOrAddAsync(ApplicationConstants.Cache.GetAllCitiesByCountryIdCacheKey + request.CountryId.ToString(), getAllCities);
            var mappedCities = _mapper.Map<List<GetCitiesByCountryIdResponse>>(cityList);
            return await Result<List<GetCitiesByCountryIdResponse>>.SuccessAsync(mappedCities);


            /*
            Func<IQueryable<City>, IIncludableQueryable<City, object>> include = a => a.Include(i => i.Country);
            var entities = await _repo.GetMultipleAsync<City, object>(asNoTracking: false, whereExpression: a => a.CountryId == request.CountryId, includeExpression: include, 
                projectExpression: select => new GetCitiesByCountryIdResponse
            {
                Code = select.Code,
                //CountryId = select.CountryId.ToString(),
                //CountryName = select.Country.Name,
                Name = select.Name
            });

            var mappedCities = _mapper.Map<List<GetCitiesByCountryIdResponse>>(entities);
            return await Result<List<GetCitiesByCountryIdResponse>>.SuccessAsync(mappedCities);*/
        }
    }
}
