using ArdaManager.Application.Interfaces.Repositories;
using ArdaManager.Domain.Entities.Inventory;
using ArdaManager.Shared.Wrapper;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ArdaManager.Application.Features.Warehouses.Queries.GetById
{
    public class GetWarehouseByIdQuery : IRequest<Result<GetWarehouseByIdResponse>>
    {
        public int Id { get; set; }
    }

    internal class GetWarehouseByIdQueryHandler : IRequestHandler<GetWarehouseByIdQuery, Result<GetWarehouseByIdResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetWarehouseByIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<GetWarehouseByIdResponse>> Handle(GetWarehouseByIdQuery query, CancellationToken cancellationToken)
        {
            var warehouse = await _unitOfWork.Repository<Warehouse>().GetByIdAsync(query.Id);
            var mappedWarehouse = _mapper.Map<GetWarehouseByIdResponse>(warehouse);
            return await Result<GetWarehouseByIdResponse>.SuccessAsync(mappedWarehouse);
        }
    }
}
