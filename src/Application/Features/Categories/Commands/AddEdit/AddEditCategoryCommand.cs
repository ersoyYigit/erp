using ArdaManager.Application.Interfaces.Repositories;
using ArdaManager.Domain.Entities.Misc;
using ArdaManager.Domain.Enums;
using ArdaManager.Shared.Constants.Application;
using ArdaManager.Shared.Wrapper;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ArdaManager.Application.Features.Categories.Commands.AddEdit
{

    public partial class AddEditCategoryCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        public string Name { get; set; }
        
        public string Description { get; set; }
        [Required]
        public CategoryType Type { get; set; }

        public int? ParentCategoryId { get; set; }
    }

    internal class AddEditCategoryCommandHandler : IRequestHandler<AddEditCategoryCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<AddEditCategoryCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public AddEditCategoryCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IStringLocalizer<AddEditCategoryCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(AddEditCategoryCommand command, CancellationToken cancellationToken)
        {
            if (command.Id == 0)
            {
                var category = _mapper.Map<Category>(command);
                await _unitOfWork.Repository<Category>().AddAsync(category);
                await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllCategoriesCacheKey);
                return await Result<int>.SuccessAsync(category.Id, _localizer["Category Saved"]);
            }
            else
            {
                var category = await _unitOfWork.Repository<Category>().GetByIdAsync(command.Id);
                if (category != null)
                {
                    category.Name = command.Name ?? category.Name;
                    category.Code = command.Code ?? category.Code;
                    category.ParentCategoryId = command.ParentCategoryId ?? category.ParentCategoryId;
                    category.Type = (command.Type == 0) ? category.Type : command.Type;
                    category.Description = command.Description ?? category.Description;
                    await _unitOfWork.Repository<Category>().UpdateAsync(category);
                    await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllCategoriesCacheKey);
                    return await Result<int>.SuccessAsync(category.Id, _localizer["Category Updated"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["Category Not Found!"]);
                }
            }
        }
    }
}
