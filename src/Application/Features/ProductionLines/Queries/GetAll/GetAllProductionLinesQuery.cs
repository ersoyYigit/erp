using ArdaManager.Application.Interfaces.Repositories;
using ArdaManager.Domain.Entities.Inventory;
using ArdaManager.Shared.Constants.Application;
using ArdaManager.Shared.Wrapper;
using AutoMapper;
using LazyCache;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ArdaManager.Application.Features.ProductionLines.Queries.GetAll
{
    public class GetAllProductionLinesQuery : IRequest<Result<List<GetAllProductionLinesResponse>>>
    {
        public GetAllProductionLinesQuery()
        {
        }
    }

    internal class GetAllProductionLinesCachedQueryHandler : IRequestHandler<GetAllProductionLinesQuery, Result<List<GetAllProductionLinesResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAppCache _cache;

        public GetAllProductionLinesCachedQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IAppCache cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<Result<List<GetAllProductionLinesResponse>>> Handle(GetAllProductionLinesQuery request, CancellationToken cancellationToken)
        {

            Expression<Func<ProductionLine, GetAllProductionLinesResponse>> expression = e => new GetAllProductionLinesResponse
            {
                Id = e.Id,
                Name = e.Name,
                Description = e.Description,
                Code = e.Code,
                //RackCount = e.Racks.Count
            };

            var data = await _unitOfWork.Repository<ProductionLine>().Entities
                   //.Include(x => x.Racks)
                   .Select(expression)
                   .ToListAsync();

            //Func<Task<List<ProductionLine>>> getAllProductionLines = () => _unitOfWork.Repository<ProductionLine>().GetAllAsync();
            //var productionLineList = await _cache.GetOrAddAsync(ApplicationConstants.Cache.GetAllProductionLinesCacheKey, getAllProductionLines);
            var mappedProductionLines = _mapper.Map<List<GetAllProductionLinesResponse>>(data);
            return await Result<List<GetAllProductionLinesResponse>>.SuccessAsync(mappedProductionLines);
        }
    }
}
