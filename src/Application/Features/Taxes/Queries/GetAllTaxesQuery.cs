using ArdaManager.Application.Interfaces.Repositories;
using ArdaManager.Domain.Entities.Misc;
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

namespace ArdaManager.Application.Features.Taxes.Queries
{
    public class GetAllTaxesResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public int Percent { get; set; }
    }

    public class GetAllTaxesQuery : IRequest<Result<List<GetAllTaxesResponse>>>
    {
        public GetAllTaxesQuery()
        {
        }
    }

    internal class GetAllTaxesCachedQueryHandler : IRequestHandler<GetAllTaxesQuery, Result<List<GetAllTaxesResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAppCache _cache;

        public GetAllTaxesCachedQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IAppCache cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<Result<List<GetAllTaxesResponse>>> Handle(GetAllTaxesQuery request, CancellationToken cancellationToken)
        {
            Func<Task<List<Tax>>> getAllTaxes = () => _unitOfWork.Repository<Tax>().GetAllAsync();
            var taxList = await _cache.GetOrAddAsync(ApplicationConstants.Cache.GetAllTaxesCacheKey, getAllTaxes);

            var mappedTaxes = taxList.Select(c => _mapper.Map<GetAllTaxesResponse>(c)).ToList();
            return await Result<List<GetAllTaxesResponse>>.SuccessAsync(mappedTaxes);
        }
    }
}
