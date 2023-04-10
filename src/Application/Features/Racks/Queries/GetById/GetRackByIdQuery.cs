using ArdaManager.Application.Interfaces.Repositories;
using ArdaManager.Domain.Entities.Inventory;
using ArdaManager.Shared.Wrapper;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ArdaManager.Application.Features.Racks.Queries.GetById
{
    public class GetRackByIdQuery : IRequest<Result<GetRackByIdResponse>>
    {
        public int Id { get; set; }
    }

    internal class GetRackByIdQueryHandler : IRequestHandler<GetRackByIdQuery, Result<GetRackByIdResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IVappRepository _repo;

        public GetRackByIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IVappRepository repo)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _repo = repo;
        }

        public async Task<Result<GetRackByIdResponse>> Handle(GetRackByIdQuery query, CancellationToken cancellationToken)
        {
            Func<IQueryable<Rack>, IIncludableQueryable<Rack, object>> include = a => a.Include(i => i.Warehouse);
            var entities = await _repo.GetSingleAsync<Rack, object>(asNoTracking: false, whereExpression: a => a.Id == query.Id, includeExpression: include, projectExpression: select => new GetRackByIdResponse
            {
                Code = select.Code,
                WarehouseId = select.WarehouseId,
                WarehouseName = select.Warehouse.Name,
                Name = select.Name,
                Description = select.Description,
                SectionCode= select.SectionCode,    
                Section = select.Section
            });
            //var rack = await _unitOfWork.Repository<Rack>().GetByIdAsync(query.Id);
            var mappedRack = _mapper.Map<GetRackByIdResponse>(entities);
            return await Result<GetRackByIdResponse>.SuccessAsync(mappedRack);
        }
    }
}
