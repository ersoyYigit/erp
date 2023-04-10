using ArdaManager.Application.Interfaces.Repositories;
using ArdaManager.Domain.Entities.Misc;
using ArdaManager.Shared.Constants.Application;
using ArdaManager.Shared.Wrapper;
using MediatR;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ArdaManager.Application.Features.Categories.Commands.Delete
{
    
    public class DeleteCategoryCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    internal class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, Result<int>>
    {
        private readonly IProductRepository _productRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IStringLocalizer<DeleteCategoryCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public DeleteCategoryCommandHandler(IUnitOfWork<int> unitOfWork, IProductRepository productRepository, IStringLocalizer<DeleteCategoryCommandHandler> localizer, ICompanyRepository companyRepository)
        {
            _unitOfWork = unitOfWork;
            _productRepository = productRepository;
            _localizer = localizer;
            _companyRepository = companyRepository;
        }

        public async Task<Result<int>> Handle(DeleteCategoryCommand command, CancellationToken cancellationToken)
        {
            var isCategoryUsedProduct = await _productRepository.IsCategoryUsed(command.Id);
            var isCategoryUsedCompany = await _companyRepository.IsCategoryUsed(command.Id);
            if (!(isCategoryUsedProduct && isCategoryUsedCompany))
            {
                var category = await _unitOfWork.Repository<Category>().GetByIdAsync(command.Id);
                if (category != null)
                {
                    await _unitOfWork.Repository<Category>().DeleteAsync(category);
                    await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllCategoriesCacheKey);
                    return await Result<int>.SuccessAsync(category.Id, _localizer["Category Deleted"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["Category Not Found!"]);
                }
            }
            else
            {
                return await Result<int>.FailAsync(_localizer["Deletion Not Allowed"]);
            }
        }
    }
}
