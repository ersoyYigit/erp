using AutoMapper;
using ArdaManager.Application.Interfaces.Repositories;
using ArdaManager.Domain.Entities.Catalog;
using ArdaManager.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ArdaManager.Application.Features.Patterns.Queries.GetById
{
    public class GetPatternByIdQuery : IRequest<Result<GetPatternByIdResponse>>
    {
        public int Id { get; set; }
    }

    internal class GetPatternByIdQueryHandler : IRequestHandler<GetPatternByIdQuery, Result<GetPatternByIdResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetPatternByIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<GetPatternByIdResponse>> Handle(GetPatternByIdQuery query, CancellationToken cancellationToken)
        {
            var pattern = await _unitOfWork.Repository<Pattern>().GetByIdAsync(query.Id);
            var mappedPattern = _mapper.Map<GetPatternByIdResponse>(pattern);
            return await Result<GetPatternByIdResponse>.SuccessAsync(mappedPattern);
        }
    }
}