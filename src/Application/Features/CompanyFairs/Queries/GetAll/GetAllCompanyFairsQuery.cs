using ArdaManager.Application.Interfaces.Repositories;
using ArdaManager.Domain.Entities.Corporation;
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

namespace ArdaManager.Application.Features.CompanyFairs.Queries.GetAll
{
    public class GetAllCompanyFairsQuery : IRequest<Result<List<GetAllCompanyFairsResponse>>>
    {
        public GetAllCompanyFairsQuery()
        {
        }
    }

    internal class GetAllCompanyFairsCachedQueryHandler : IRequestHandler<GetAllCompanyFairsQuery, Result<List<GetAllCompanyFairsResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAppCache _cache;
        private readonly IVappRepository _repo;

        public GetAllCompanyFairsCachedQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IAppCache cache, IVappRepository repo)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cache = cache;
            _repo = repo;
        }

        public async Task<Result<List<GetAllCompanyFairsResponse>>> Handle(GetAllCompanyFairsQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<CompanyFair, GetAllCompanyFairsResponse>> expression = e => new GetAllCompanyFairsResponse
            {
                Id = e.Id,
                CompanyId = e.CompanyId,
                CompanyName = e.Company.Name,
                FairId = e.FairId,
                FairName = e.Fair.Name,
                FairDate = e.Fair.Date,
                FairDescription = e.Fair.Description,
                Type = e.Fair.Type
                
            };

            var data = await _unitOfWork.Repository<CompanyFair>().Entities
                   .Include(x => x.Fair)
                   .Include(y => y.Company)
                   .Select(expression)
                   .ToListAsync();

            var mappedCompanyFairs = _mapper.Map<List<GetAllCompanyFairsResponse>>(data);
            return await Result<List<GetAllCompanyFairsResponse>>.SuccessAsync(mappedCompanyFairs);

        }
    }
}
