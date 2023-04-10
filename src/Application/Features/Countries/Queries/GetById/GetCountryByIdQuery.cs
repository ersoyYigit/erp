using ArdaManager.Application.Features.Patterns.Queries.GetById;
using ArdaManager.Application.Interfaces.Repositories;
using ArdaManager.Domain.Entities.Addressing;
using ArdaManager.Shared.Wrapper;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ArdaManager.Application.Features.Countries.Queries.GetById
{
    
    public class GetCountryByIdQuery : IRequest<Result<GetCountryByIdResponse>>
    {
        public int Id { get; set; }
    }

    internal class GetCountryByIdQueryHandler : IRequestHandler<GetCountryByIdQuery, Result<GetCountryByIdResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetCountryByIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<GetCountryByIdResponse>> Handle(GetCountryByIdQuery query, CancellationToken cancellationToken)
        {
            var country = await _unitOfWork.Repository<Country>().GetByIdAsync(query.Id);
            var mappedCountry = _mapper.Map<GetCountryByIdResponse>(country);
            return await Result<GetCountryByIdResponse>.SuccessAsync(mappedCountry);
        }
    }
}
