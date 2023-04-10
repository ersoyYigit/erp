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

namespace ArdaManager.Application.Features.Currencies.Queries
{
    public class GetAllCurrenciesResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Sign { get; set; }
        public string CustomCode { get; set; }
    }

    public class GetAllCurrenciesQuery : IRequest<Result<List<GetAllCurrenciesResponse>>>
    {
        public GetAllCurrenciesQuery()
        {
        }
    }

    internal class GetAllCurrenciesCachedQueryHandler : IRequestHandler<GetAllCurrenciesQuery, Result<List<GetAllCurrenciesResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAppCache _cache;

        public GetAllCurrenciesCachedQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IAppCache cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<Result<List<GetAllCurrenciesResponse>>> Handle(GetAllCurrenciesQuery request, CancellationToken cancellationToken)
        {
            Func<Task<List<Currency>>> getAllCurrencies = () => _unitOfWork.Repository<Currency>().GetAllAsync();
            var currencyList = await _cache.GetOrAddAsync(ApplicationConstants.Cache.GetAllCurrenciesCacheKey, getAllCurrencies);

            var mappedCurrencies = currencyList.Select(c => _mapper.Map<GetAllCurrenciesResponse>(c)).ToList();
            return await Result<List<GetAllCurrenciesResponse>>.SuccessAsync(mappedCurrencies);
        }
    }
}
