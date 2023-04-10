using ArdaManager.Application.Extensions;
using ArdaManager.Application.Interfaces.Repositories;
using ArdaManager.Domain.Entities.Inventory;
using ArdaManager.Shared.Constants.Application;
using ArdaManager.Shared.Wrapper;
using AutoMapper;
using LazyCache;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ArdaManager.Application.Features.Racks.Queries.GetAll
{
    public class GetAllRacksQuery : IRequest<Result<List<GetAllRacksResponse>>>
    {
        public GetAllRacksQuery()
        {
        }
    }

    internal class GetAllRacksCachedQueryHandler : IRequestHandler<GetAllRacksQuery, Result<List<GetAllRacksResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAppCache _cache;
        private readonly IVappRepository _repo;

        public GetAllRacksCachedQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IAppCache cache, IVappRepository repo)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cache = cache;
            _repo = repo;
        }

        public async Task<Result<List<GetAllRacksResponse>>> Handle(GetAllRacksQuery request, CancellationToken cancellationToken)
        {
            
            Expression<Func<Rack, GetAllRacksResponse>> expression = e => new GetAllRacksResponse
            {
                Id = e.Id,
                Name = e.Name,
                Description = e.Description,
                Code = e.Code, 
                WarehouseId = e.WarehouseId,
                WarehouseName = e.Warehouse.Name,
                Section = e.Section,
                SectionCode = e.SectionCode
                
            };

            var data = await _unitOfWork.Repository<Rack>().Entities
                   .Include(x=>x.Warehouse)
                   .Select(expression)
                   .ToListAsync();

            var mappedRacks = _mapper.Map<List<GetAllRacksResponse>>(data);
            return await Result<List<GetAllRacksResponse>>.SuccessAsync(mappedRacks);
        }
    }
}
