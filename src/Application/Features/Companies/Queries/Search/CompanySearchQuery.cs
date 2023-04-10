using ArdaManager.Application.Features.Companies.Queries.Search;
using ArdaManager.Application.Interfaces.Repositories;
using ArdaManager.Domain.Entities.Catalog;
using ArdaManager.Domain.Entities.Corporation;
using ArdaManager.Domain.Enums;
using ArdaManager.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ArdaManager.Application.Features.Companies.Queries.Search
{
    public class CompanySearchQuery : IRequest<Result<List<CompanySearchResultDto>>>
    {
        public string SearchTerm { get; set; }
        public CompanyType Type { get; set; }
    }

    public class CompanySearchQueryHandler : IRequestHandler<CompanySearchQuery, Result<List<CompanySearchResultDto>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;

        public CompanySearchQueryHandler(IUnitOfWork<int> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<List<CompanySearchResultDto>>> Handle(CompanySearchQuery query, CancellationToken cancellationToken)
        {
            var searchTerm = query.SearchTerm;
            var type = query.Type;

            Expression<Func<Company, bool>> filter = p => (string.IsNullOrEmpty(searchTerm) || p.Name.Contains(searchTerm) || p.Code.Contains(searchTerm)) && p.Type == type;

            var products = await _unitOfWork.Repository<Company>()
                .GetAllAsync(
                    predicate: filter,
                    include: p => p.Include(x => x.Category),
                    take: 25
                );

            var result = products.Select(p => new CompanySearchResultDto
            {
                CompanyId = p.Id,
                Name = p.Name,
                Code = p.Code,
                Type = p.Type,
                CategoryId = p.Category?.Id,
                CategoryName = p.Category?.Name
            }).ToList();

            return await Result<List<CompanySearchResultDto>>.SuccessAsync(result);
        }
    }
}
