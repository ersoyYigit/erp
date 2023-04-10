using System.ComponentModel.DataAnnotations;
using AutoMapper;
using ArdaManager.Application.Interfaces.Repositories;
using ArdaManager.Domain.Entities.Catalog;
using ArdaManager.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using ArdaManager.Shared.Constants.Application;

namespace ArdaManager.Application.Features.ProductCategories.Commands.AddEdit
{
  public partial class AddEditProductCategoryCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
    }
    
    internal class AddEditProductCategoryCommandHandler : IRequestHandler<AddEditProductCategoryCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<AddEditProductCategoryCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public AddEditProductCategoryCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IStringLocalizer<AddEditProductCategoryCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(AddEditProductCategoryCommand command, CancellationToken cancellationToken)
        {
            if (command.Id == 0)
            {
                var productCategory = _mapper.Map<ProductCategory>(command);
                await _unitOfWork.Repository<ProductCategory>().AddAsync(productCategory);
                await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllProductCategoriesCacheKey);
                return await Result<int>.SuccessAsync(productCategory.Id, _localizer["ProductCategory Saved"]);
            }
            else
            {
                var productCategory = await _unitOfWork.Repository<ProductCategory>().GetByIdAsync(command.Id);
                if (productCategory != null)
                {
                    productCategory.Name = command.Name ?? productCategory.Name;
                    
                    productCategory.Description = command.Description ?? productCategory.Description;
                    await _unitOfWork.Repository<ProductCategory>().UpdateAsync(productCategory);
                    await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllProductCategoriesCacheKey);
                    return await Result<int>.SuccessAsync(productCategory.Id, _localizer["ProductCategory Updated"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["ProductCategory Not Found!"]);
                }
            }
        }
    }
}
