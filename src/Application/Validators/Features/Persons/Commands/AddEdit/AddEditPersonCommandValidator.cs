using ArdaManager.Application.Features.Persons.Commands.AddEdit;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ArdaManager.Application.Validators.Features.Persons.Commands.AddEdit
{
    public  class AddEditPersonCommandValidator : AbstractValidator<AddEditPersonCommand>
    {
        public AddEditPersonCommandValidator(IStringLocalizer<AddEditPersonCommandValidator> localizer)
        {
            RuleFor(request => request.Name)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Name is required!"]);
            RuleFor(request => request.Code)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Code is required!"]);
            RuleFor(request => request.CategoryId)
                .GreaterThan(0).WithMessage(x => localizer["Category is required!"]);

        }
    }
}
