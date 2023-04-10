using ArdaManager.Application.Features.MeasurementUnits.Queries.GetAll;
using ArdaManager.Application.Interfaces.Repositories;
using ArdaManager.Domain.Entities.Catalog;
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

namespace ArdaManager.Application.Features.MeasurementUnits.Queries.GetAll
{
    public class GetAllMeasurementUnitsQuery : IRequest<Result<List<GetAllMeasurementUnitsResponse>>>
    {
        public GetAllMeasurementUnitsQuery()
        {
        }
    }

    internal class GetAllMUsCachedQueryHandler : IRequestHandler<GetAllMeasurementUnitsQuery, Result<List<GetAllMeasurementUnitsResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAppCache _cache;

        public GetAllMUsCachedQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IAppCache cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<Result<List<GetAllMeasurementUnitsResponse>>> Handle(GetAllMeasurementUnitsQuery request, CancellationToken cancellationToken)
        {
            Func<Task<List<MeasurementUnit>>> getAllMUs = () => _unitOfWork.Repository<MeasurementUnit>().GetAllAsync();
            var muList = await _cache.GetOrAddAsync(ApplicationConstants.Cache.GetAllMUsCacheKey, getAllMUs);
            var mappedMUs = _mapper.Map<List<GetAllMeasurementUnitsResponse>>(muList);
            return await Result<List<GetAllMeasurementUnitsResponse>>.SuccessAsync(mappedMUs);
        }
    }
}
