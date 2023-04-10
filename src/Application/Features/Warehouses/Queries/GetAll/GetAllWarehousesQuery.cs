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

namespace ArdaManager.Application.Features.Warehouses.Queries.GetAll
{
    public class GetAllWarehousesQuery : IRequest<Result<List<GetAllWarehousesResponse>>>
    {
        public GetAllWarehousesQuery()
        {
        }
    }

    internal class GetAllWarehousesCachedQueryHandler : IRequestHandler<GetAllWarehousesQuery, Result<List<GetAllWarehousesResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAppCache _cache;

        public GetAllWarehousesCachedQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IAppCache cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<Result<List<GetAllWarehousesResponse>>> Handle(GetAllWarehousesQuery request, CancellationToken cancellationToken)
        {

            Expression<Func<Warehouse, GetAllWarehousesResponse>> expression = e => new GetAllWarehousesResponse
            {
                Id = e.Id,
                Name = e.Name,
                Description = e.Description,
                Code = e.Code,
                RackCount = e.Racks.Count
            };

            var data = await _unitOfWork.Repository<Warehouse>().Entities
                   //.Include(x => x.Racks)
                   .Select(expression)
                   .ToListAsync();

            //Func<Task<List<Warehouse>>> getAllWarehouses = () => _unitOfWork.Repository<Warehouse>().GetAllAsync();
            //var warehouseList = await _cache.GetOrAddAsync(ApplicationConstants.Cache.GetAllWarehousesCacheKey, getAllWarehouses);
            var mappedWarehouses = _mapper.Map<List<GetAllWarehousesResponse>>(data);
            return await Result<List<GetAllWarehousesResponse>>.SuccessAsync(mappedWarehouses);
        }
    }
}
