using ArdaManager.Application.Extensions;
using ArdaManager.Application.Interfaces.Repositories;
using ArdaManager.Application.Specifications.Corporation;
using ArdaManager.Domain.Entities.Corporation;
using ArdaManager.Shared.Wrapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ArdaManager.Application.Features.Companies.Queries.GetAllPaged
{
    
    public class GetAllCompaniesQuery : IRequest<PaginatedResult<GetAllPagedCompaniesResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SearchString { get; set; }
        public string[] OrderBy { get; set; } // of the form fieldname [ascending|descending],fieldname [ascending|descending]...

        public GetAllCompaniesQuery(int pageNumber, int pageSize, string searchString, string orderBy)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            SearchString = searchString;
            if (!string.IsNullOrWhiteSpace(orderBy))
            {
                OrderBy = orderBy.Split(',');
            }
        }
    }

    internal class GetAllCompaniesQueryHandler : IRequestHandler<GetAllCompaniesQuery, PaginatedResult<GetAllPagedCompaniesResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;

        public GetAllCompaniesQueryHandler(IUnitOfWork<int> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginatedResult<GetAllPagedCompaniesResponse>> Handle(GetAllCompaniesQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Company, GetAllPagedCompaniesResponse>> expression = e => new GetAllPagedCompaniesResponse
            {
                Id = e.Id,
                Name = e.Name,
                Code = e.Code,
                Address = e.Address,
                CityId = (int)e.CityId,
                CityName = e.City.Name,
                CountryId = e.City.Country.Id,
                CountryName = e.City.Country.Name.ToString(),
                Fax = e.Fax,
                Notes = e.Notes,
                Telephone = e.Telephone,
                Telephone2 = e.Telephone2,
                Type = e.Type,
                Vat = e.Vat,    
                WebSite = e.WebSite,
                RepresantativeId = e.RepresantativeId,
                RepresantativeName = e.Represantative.FullName
            };
            var companyFilterSpec = new CompanyFilterSpecification(request.SearchString);
            if (request.OrderBy?.Any() != true)
            {
                var data = await _unitOfWork.Repository<Company>().Entities
                   .Specify(companyFilterSpec)
                   .Select(expression)
                   .ToPaginatedListAsync(request.PageNumber, request.PageSize);
                return data;
            }
            else
            {
                var ordering = string.Join(",", request.OrderBy); // of the form fieldname [ascending|descending], ...
                var data = await _unitOfWork.Repository<Company>().Entities
                   .Specify(companyFilterSpec)
                   .OrderBy(ordering) // require system.linq.dynamic.core
                   .Select(expression)
                   .ToPaginatedListAsync(request.PageNumber, request.PageSize);
                return data;

            }
        }
    }
}
