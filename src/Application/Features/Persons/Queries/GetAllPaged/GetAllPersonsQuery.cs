using ArdaManager.Application.Extensions;
using ArdaManager.Application.Interfaces.Repositories;
using ArdaManager.Application.Specifications.Human;
using ArdaManager.Domain.Entities.Human;
using ArdaManager.Shared.Wrapper;
using MediatR;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;

namespace ArdaManager.Application.Features.Persons.Queries.GetAllPaged
{
    public class GetAllPersonsQuery : IRequest<PaginatedResult<GetAllPagedPersonsResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SearchString { get; set; }
        public string[] OrderBy { get; set; } // of the form fieldname [ascending|descending],fieldname [ascending|descending]...

        public GetAllPersonsQuery(int pageNumber, int pageSize, string searchString, string orderBy)
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

    internal class GetAllPersonsQueryHandler : IRequestHandler<GetAllPersonsQuery, PaginatedResult<GetAllPagedPersonsResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;

        public GetAllPersonsQueryHandler(IUnitOfWork<int> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginatedResult<GetAllPagedPersonsResponse>> Handle(GetAllPersonsQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Person, GetAllPagedPersonsResponse>> expression = e => new GetAllPagedPersonsResponse
            {
                Id = e.Id,
                Name = e.Name,
                BCard = e.BCard,
                Code = e.Code,
                Email = e.Email,
                EmailSecondary = e.EmailSecondary,
                Fax = e.Fax,
                SurName = e.Surname,
                Gender = e.Gender,
                Note = e.Note,
                ImageDataURL = e.ImageDataURL,
                MiddleName = e.MiddleName,
                Mobile = e.Mobile,
                Status = e.Status,
                Telephone = e.Telephone,
                TelephoneExt = e.TelephoneExt,
                Title = e.Title,
                Description = e.Description,
                FullName = e.FullName,
                
                CategoryName = e.Category.Name,
                CategoryId = e.CategoryId
            };
            var personFilterSpec = new PersonFilterSpecification(request.SearchString);
            if (request.OrderBy?.Any() != true)
            {
                var data = await _unitOfWork.Repository<Person>().Entities
                   .Specify(personFilterSpec)
                   .Select(expression)
                   .ToPaginatedListAsync(request.PageNumber, request.PageSize);
                return data;
            }
            else
            {
                var ordering = string.Join(",", request.OrderBy); // of the form fieldname [ascending|descending], ...
                var data = await _unitOfWork.Repository<Person>().Entities
                   .Specify(personFilterSpec)
                   .OrderBy(ordering) // require system.linq.dynamic.core
                   .Select(expression)
                   .ToPaginatedListAsync(request.PageNumber, request.PageSize);
                return data;

            }
        }
    }
}
