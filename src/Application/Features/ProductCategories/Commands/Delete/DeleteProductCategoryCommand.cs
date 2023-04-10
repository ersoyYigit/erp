using MediatR;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ArdaManager.Application.Interfaces.Repositories;
using ArdaManager.Domain.Entities.Catalog;
using ArdaManager.Shared.Constants.Application;
using ArdaManager.Shared.Wrapper;

namespace ArdaManager.Application.Features.ProductCategories.Commands.Delete
{
    public class DeleteProductCategoryCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    internal class DeleteProductCategoryCommandHandler : IRequestHandler<DeleteProductCategoryCommand, Result<int>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IStringLocalizer<DeleteProductCategoryCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public DeleteProductCategoryCommandHandler(IUnitOfWork<int> unitOfWork, IProductRepository productRepository, IStringLocalizer<DeleteProductCategoryCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _productRepository = productRepository;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(DeleteProductCategoryCommand command, CancellationToken cancellationToken)
        {
            var isProductCategoryUsed = await _productRepository.IsProductCategoryUsed(command.Id);
            if (!isProductCategoryUsed)
            {
                var productCategory = await _unitOfWork.Repository<ProductCategory>().GetByIdAsync(command.Id);
                if (productCategory != null)
                {
                    await _unitOfWork.Repository<ProductCategory>().DeleteAsync(productCategory);
                    await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllProductCategoriesCacheKey);
                    return await Result<int>.SuccessAsync(productCategory.Id, _localizer["ProductCategory Deleted"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["ProductCategory Not Found!"]);
                }
            }
            else
            {
                return await Result<int>.FailAsync(_localizer["Deletion Not Allowed"]);
            }
        }
    }
}
