using ArdaManager.Application.Interfaces.Repositories;
using ArdaManager.Domain.Entities.Corporation;
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

namespace ArdaManager.Application.Features.Fairs.Queries.GetAll
{
    public class GetAllFairsQuery : IRequest<Result<List<GetAllFairsResponse>>>
    {
        public GetAllFairsQuery()
        {
        }
    }

    internal class GetAllFairsCachedQueryHandler : IRequestHandler<GetAllFairsQuery, Result<List<GetAllFairsResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAppCache _cache;
        private readonly IVappRepository _repo;

        public GetAllFairsCachedQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IAppCache cache, IVappRepository repo)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cache = cache;
            _repo = repo;
        }

        public async Task<Result<List<GetAllFairsResponse>>> Handle(GetAllFairsQuery request, CancellationToken cancellationToken)
        {

            Expression<Func<Fair, GetAllFairsResponse>> expression = e => new GetAllFairsResponse
            {
                Id = e.Id,
                Name = e.Name,
                Description = e.Description,
                Code = e.Code,
                Type = e.Type,
                Date = e.Date
            };

            var data = await _unitOfWork.Repository<Fair>().Entities
                   .Select(expression)
                   .ToListAsync();

            var mappedFairs = _mapper.Map<List<GetAllFairsResponse>>(data);
            return await Result<List<GetAllFairsResponse>>.SuccessAsync(mappedFairs);
        }
    }
}
