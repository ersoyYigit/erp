using ArdaManager.Application.Features.Categories.Commands.AddEdit;
using ArdaManager.Application.Features.RecipeItems.Commands.AddEdit;
using ArdaManager.Application.Validators.Features.Categories.Commands.AddEdit;
using FluentValidation;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Application.Validators.Features.RecipeItems.Commands.AddEdit
{
    public class AddEditRecipeItemCommandValidator : AbstractValidator<AddEditRecipeItemCommand>
    {
        public AddEditRecipeItemCommandValidator(IStringLocalizer<AddEditRecipeItemCommandValidator> localizer)
        {
            RuleFor(request => request.OwnerProductId)
                .GreaterThan(0).WithMessage(x => localizer["Basic Product required!"]);
            RuleFor(request => request.RecipeProductId)
                .GreaterThan(0).WithMessage(x => localizer["Product is required!"]);
            RuleFor(request => request.Quantity)
                .GreaterThan(0).WithMessage(x => localizer["Quantity must be greater than 0 !"]);

        }
    }
}
