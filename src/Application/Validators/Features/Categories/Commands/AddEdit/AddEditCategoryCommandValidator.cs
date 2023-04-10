using ArdaManager.Application.Features.Categories.Commands.AddEdit;
using ArdaManager.Domain.Enums;
using FluentValidation;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Application.Validators.Features.Categories.Commands.AddEdit
{
    public class AddEditCategoryCommandValidator : AbstractValidator<AddEditCategoryCommand>
    {
        public AddEditCategoryCommandValidator(IStringLocalizer<AddEditCategoryCommandValidator> localizer)
        {
            RuleFor(request => request.Code)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Kod gereklidir!"]);
            RuleFor(request => request.Name)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Ad gereklidir!"]);





        }
    }
}
