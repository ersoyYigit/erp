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

namespace ArdaManager.Application.Features.ProductionLines.Queries.GetById
{
    public class GetProductionLineByIdQuery : IRequest<Result<GetProductionLineByIdResponse>>
    {
        public int Id { get; set; }
    }

    internal class GetProductionLineByIdQueryHandler : IRequestHandler<GetProductionLineByIdQuery, Result<GetProductionLineByIdResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetProductionLineByIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<GetProductionLineByIdResponse>> Handle(GetProductionLineByIdQuery query, CancellationToken cancellationToken)
        {
            var productionLine = await _unitOfWork.Repository<ProductionLine>().GetByIdAsync(query.Id);
            var mappedProductionLine = _mapper.Map<GetProductionLineByIdResponse>(productionLine);
            return await Result<GetProductionLineByIdResponse>.SuccessAsync(mappedProductionLine);
        }
    }
}
