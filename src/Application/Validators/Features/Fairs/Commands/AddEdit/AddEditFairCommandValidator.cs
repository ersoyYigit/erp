using ArdaManager.Application.Features.Patterns.Commands.AddEdit;
using ArdaManager.Application.Features.Fairs.Commands.AddEdit;
using FluentValidation;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Application.Validators.Features.Fairs.Commands.AddEdit
{
    public class AddEditFairCommandValidator : AbstractValidator<AddEditFairCommand>
    {
        public AddEditFairCommandValidator(IStringLocalizer<AddEditFairCommandValidator> localizer)
        {
            RuleFor(request => request.Name)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Name is required!"]);
            RuleFor(request => request.Description)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Description is required!"]);
            RuleFor(request => request.Code)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Code is required!"]);
        }
    }
}
