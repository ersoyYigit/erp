using ArdaManager.Application.Features.Patterns.Queries.GetById;
using ArdaManager.Application.Features.Cities.Queries.GetById;
using ArdaManager.Application.Interfaces.Repositories;
using ArdaManager.Domain.Entities.Addressing;
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

namespace ArdaManager.Application.Features.Cities.Queries.GetById
{
    
    public class GetCityByIdQuery : IRequest<Result<GetCityByIdResponse>>
    {
        public int Id { get; set; }
    }

    internal class GetCityByIdQueryHandler : IRequestHandler<GetCityByIdQuery, Result<GetCityByIdResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IVappRepository _repo;

        public GetCityByIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IVappRepository repo)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _repo = repo;
        }

        public async Task<Result<GetCityByIdResponse>> Handle(GetCityByIdQuery query, CancellationToken cancellationToken)
        {
            Func<IQueryable<City>, IIncludableQueryable<City, object>> include = a => a.Include(i => i.Country);
            var entities = await _repo.GetSingleAsync<City, object>(asNoTracking: false, whereExpression: a => a.Id == query.Id, includeExpression: include, projectExpression: select => new GetCityByIdResponse
            {
                Code = select.Code,
                CountryId = select.CountryId.ToString(),
                CountryName = select.Country.Name,
                Name = select.Name
            });
            //var city = await _unitOfWork.Repository<City>().GetByIdAsync(query.Id);
            var mappedCity = _mapper.Map<GetCityByIdResponse>(entities);
            return await Result<GetCityByIdResponse>.SuccessAsync(mappedCity);
        }
    }
}
