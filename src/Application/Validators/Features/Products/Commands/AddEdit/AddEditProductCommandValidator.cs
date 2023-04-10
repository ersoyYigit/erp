using ArdaManager.Application.Features.Products.Commands.AddEdit;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ArdaManager.Application.Validators.Features.Products.Commands.AddEdit
{
    public class AddEditTemplateCommandValidator : AbstractValidator<AddEditProductCommand>
    {
        public AddEditTemplateCommandValidator(IStringLocalizer<AddEditTemplateCommandValidator> localizer)
        {
            RuleFor(request => request.Name)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Name is required!"]);
            RuleFor(request => request.Code)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Code is required!"]);

            RuleFor(request => request.MeasurementUnitId).GreaterThan(0).WithMessage(x => localizer["Ürün için ölçü birimi gereklidir."]);

            //RuleFor(request => request.PatternId)
            //.GreaterThan(0).WithMessage(x => localizer["Pattern is required!"]);
            //RuleFor(request => request.CategoryId).GreaterThan(0).WithMessage(x => localizer["Product Category is required!"]);
            //RuleFor(request => request.Rate)
            //.GreaterThan(0).WithMessage(x => localizer["Rate must be greater than 0"]);
        }
    }
}