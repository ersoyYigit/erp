using ArdaManager.Application.Features.Racks.Commands.AddEdit;
using FluentValidation;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Application.Validators.Features.Racks.Commands.AddEdit
{
    public  class AddEditRackCommandValidator : AbstractValidator<AddEditRackCommand>
    {
        public AddEditRackCommandValidator(IStringLocalizer<AddEditRackCommandValidator> localizer)
        {
            RuleFor(request => request.Name)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Name is required!"]);
            
            RuleFor(request => request.WarehouseId)
                .GreaterThan(0).WithMessage(x => localizer["Warehouse is required!"]);
        }
    }
}
