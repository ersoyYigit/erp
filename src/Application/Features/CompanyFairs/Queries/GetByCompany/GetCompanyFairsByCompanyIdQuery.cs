using ArdaManager.Application.Features.CompanyFairs.Queries.GetById;
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
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ArdaManager.Application.Features.CompanyFairs.Queries.GetByCompany
{
    public class GetCompanyFairsByCompanyIdQuery : IRequest<Result<List<GetCompanyFairsByCompanyIdResponse>>>
    {
        public int CompanyId { get; set; }
    }

    internal class GetCompanyFairsByCompanyIdResponseHandler : IRequestHandler<GetCompanyFairsByCompanyIdQuery, Result<List<GetCompanyFairsByCompanyIdResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IVappRepository _repo;

        public GetCompanyFairsByCompanyIdResponseHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IVappRepository repo)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _repo = repo;
        }

        public async Task<Result<List<GetCompanyFairsByCompanyIdResponse>>> Handle(GetCompanyFairsByCompanyIdQuery query, CancellationToken cancellationToken)
        {
            Expression<Func<CompanyFair, GetCompanyFairsByCompanyIdResponse>> expression = e => new GetCompanyFairsByCompanyIdResponse
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
                   .Where(predicate => predicate.CompanyId == query.CompanyId)
                   .ToListAsync();

            var mappedCompanyFairs = _mapper.Map<List<GetCompanyFairsByCompanyIdResponse>>(data);
            return await Result<List<GetCompanyFairsByCompanyIdResponse>>.SuccessAsync(mappedCompanyFairs);


        }
    }
}
