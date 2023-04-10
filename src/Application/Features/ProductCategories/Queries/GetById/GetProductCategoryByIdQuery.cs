using AutoMapper;
using ArdaManager.Application.Interfaces.Repositories;
using ArdaManager.Domain.Entities.Catalog;
using ArdaManager.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ArdaManager.Application.Features.ProductCategories.Queries.GetById
{
    public class GetProductCategoryByIdQuery : IRequest<Result<GetProductCategoryByIdResponse>>
    {
        public int Id { get; set; }
    }

    internal class GetProductByIdQueryHandler : IRequestHandler<GetProductCategoryByIdQuery, Result<GetProductCategoryByIdResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetProductByIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<GetProductCategoryByIdResponse>> Handle(GetProductCategoryByIdQuery query, CancellationToken cancellationToken)
        {
            var productCategory = await _unitOfWork.Repository<ProductCategory>().GetByIdAsync(query.Id);
            var mappedProductCategory = _mapper.Map<GetProductCategoryByIdResponse>(productCategory);
            return await Result<GetProductCategoryByIdResponse>.SuccessAsync(mappedProductCategory);
        }
    }
}