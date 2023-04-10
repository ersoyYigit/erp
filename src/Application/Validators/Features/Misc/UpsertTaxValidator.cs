using ArdaManager.Application.Features.Categories.Commands.AddEdit;
using ArdaManager.Application.Features.Taxes.Commands;
using ArdaManager.Application.Validators.Features.Categories.Commands.AddEdit;
using FluentValidation;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Application.Validators.Features.Misc
{
    public class UpsertTaxValidator : AbstractValidator<UpsertTaxCommand>
    {
        public UpsertTaxValidator()
        {
            RuleFor(request => request.Code)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage("Kod Gereklidir");
            //RuleFor(request => request.Percent).GreaterThan(0).WithMessage("Vergi dilimi oranı gereklidir");

        }
    }
}
