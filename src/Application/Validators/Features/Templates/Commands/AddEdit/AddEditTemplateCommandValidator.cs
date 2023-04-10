using ArdaManager.Application.Features.Products.Commands.AddEdit;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ArdaManager.Application.Validators.Features.Templates.Commands.AddEdit
{
    public class AddEditTemplateCommandValidator : AbstractValidator<AddEditProductCommand>
    {
        public AddEditTemplateCommandValidator(IStringLocalizer<AddEditTemplateCommandValidator> localizer)
        {
            
            RuleFor(request => request.Code)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Code is required!"]);
            

            //RuleFor(request => request.PatternId)
            //.GreaterThan(0).WithMessage(x => localizer["Pattern is required!"]);
            //RuleFor(request => request.CategoryId).GreaterThan(0).WithMessage(x => localizer["Product Category is required!"]);
            //RuleFor(request => request.Rate)
                //.GreaterThan(0).WithMessage(x => localizer["Rate must be greater than 0"]);
        }
    }
}