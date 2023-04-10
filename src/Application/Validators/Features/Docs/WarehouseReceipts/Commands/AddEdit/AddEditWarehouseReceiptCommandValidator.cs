using ArdaManager.Application.Features.Categories.Commands.AddEdit;
using ArdaManager.Application.Features.Docs.WarehouseReceipts.Commands.AddEdit;
using ArdaManager.Application.Validators.Features.Categories.Commands.AddEdit;
using FluentValidation;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Application.Validators.Features.Docs.WarehouseReceipts.Commands.AddEdit
{
    public class AddEditWarehouseReceiptCommandValidator : AbstractValidator<AddEditWarehouseReceiptCommand>
    {
        public AddEditWarehouseReceiptCommandValidator(IStringLocalizer<AddEditCategoryCommandValidator> localizer)
        {
            RuleFor(request => request.DocNo)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Belge numarası gereklidir!"]);
            RuleFor(request => request.WarehouseName)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Depo gereklidir!"]);


            RuleFor(request => request.WarehouseId).GreaterThan(0).WithMessage(x => localizer["Depo seçimi zorunludur."]);


        }
    }
}
