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

namespace ArdaManager.Application.Features.CompanyFairs.Queries.GetById
{
    public class GetCompanyFairByIdQuery : IRequest<Result<GetCompanyFairByIdResponse>>
    {
        public int Id { get; set; }
    }

    internal class GetCompanyFairByIdQueryHandler : IRequestHandler<GetCompanyFairByIdQuery, Result<GetCompanyFairByIdResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IVappRepository _repo;

        public GetCompanyFairByIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IVappRepository repo)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _repo = repo;
        }

        public async Task<Result<GetCompanyFairByIdResponse>> Handle(GetCompanyFairByIdQuery query, CancellationToken cancellationToken)
        {
            Func<IQueryable<CompanyFair>, IIncludableQueryable<CompanyFair, object>> include = a => a.Include(i => i.Fair).Include(i => i.Company);
            var entities = await _repo.GetSingleAsync<CompanyFair, object>(asNoTracking: false, whereExpression: a => a.Id == query.Id, includeExpression: include, projectExpression: select => new GetCompanyFairByIdResponse
            {
                FairId = select.FairId,
                FairName = select.Fair.Name,
                CompanyId = select.CompanyId,
                CompanyName = select.Company.Name,
                FairDate = select.Fair.Date,
                FairDescription = select.Fair.Description,
                Type = select.Fair.Type
            });
            //var companyFair = await _unitOfWork.Repository<CompanyFair>().GetByIdAsync(query.Id);
            var mappedCompanyFair = _mapper.Map<GetCompanyFairByIdResponse>(entities);
            return await Result<GetCompanyFairByIdResponse>.SuccessAsync(mappedCompanyFair);
        }
    }
}
