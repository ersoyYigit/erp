using FluentValidation;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArdaManager.Application.Features.ProductCategories.Commands.AddEdit;

namespace ArdaManager.Application.Validators.Features.ProductCategories.Commands.AddEdit
{
    public class AddEditProductCategoryCommandValidator : AbstractValidator<AddEditProductCategoryCommand>
    {
        public AddEditProductCategoryCommandValidator(IStringLocalizer<AddEditProductCategoryCommandValidator> localizer)
        {
            RuleFor(request => request.Name)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Name is required!"]);
            RuleFor(request => request.Description)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Description is required!"]);
            
        }
    }
}
