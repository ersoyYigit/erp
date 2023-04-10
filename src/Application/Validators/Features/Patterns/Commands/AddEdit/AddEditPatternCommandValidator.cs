using ArdaManager.Application.Features.Patterns.Commands.AddEdit;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ArdaManager.Application.Validators.Features.Patterns.Commands.AddEdit
{
    public class AddEditPatternCommandValidator : AbstractValidator<AddEditPatternCommand>
    {
        public AddEditPatternCommandValidator(IStringLocalizer<AddEditPatternCommandValidator> localizer)
        {
            RuleFor(request => request.Name)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Name is required!"]);
            RuleFor(request => request.Description)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Description is required!"]);
            RuleFor(request => request.Tax)
                .GreaterThan(0).WithMessage(x => localizer["Tax must be greater than 0"]);
        }
    }
}