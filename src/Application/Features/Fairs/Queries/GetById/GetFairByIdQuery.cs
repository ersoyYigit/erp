using ArdaManager.Application.Interfaces.Repositories;
using ArdaManager.Domain.Entities.Corporation;
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

namespace ArdaManager.Application.Features.Fairs.Queries.GetById
{
    public class GetFairByIdQuery : IRequest<Result<GetFairByIdResponse>>
    {
        public int Id { get; set; }
    }

    internal class GetFairByIdQueryHandler : IRequestHandler<GetFairByIdQuery, Result<GetFairByIdResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IVappRepository _repo;

        public GetFairByIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IVappRepository repo)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _repo = repo;
        }

        public async Task<Result<GetFairByIdResponse>> Handle(GetFairByIdQuery query, CancellationToken cancellationToken)
        {
            var fair = await _unitOfWork.Repository<Fair>().GetByIdAsync(query.Id);
            var mappedFair = _mapper.Map<GetFairByIdResponse>(fair);
            return await Result<GetFairByIdResponse>.SuccessAsync(mappedFair);
        }
    }
}
