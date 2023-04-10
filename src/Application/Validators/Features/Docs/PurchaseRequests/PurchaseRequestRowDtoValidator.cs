using ArdaManager.Application.Features.Docs.PurchaseRequests.Commands.AddEdit;
using ArdaManager.Application.Features.Docs.PurchaseRequests.Queries;
using ArdaManager.Application.Features.Docs.WarehouseReceipts.Commands.AddEdit;
using ArdaManager.Application.Validators.Features.Categories.Commands.AddEdit;
using FluentValidation;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Application.Validators.Features.Docs.PurchaseRequests
{
    public class PurchaseRequestRowDtoValidator : AbstractValidator<PurchaseRequestRowDto>
    {
        public PurchaseRequestRowDtoValidator(IStringLocalizer<AddEditCategoryCommandValidator> localizer)
        {
            RuleFor(row => row.ProductCode)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => "Belge numarası gereklidir!");

            RuleFor(row => row.ProductName)
                .NotEmpty().WithMessage("Ürün adı gereklidir!");

            RuleFor(row => row.Quantity)
                .GreaterThan(0).WithMessage("Miktar 0'dan büyük olmalıdır!");

            RuleFor(row => row.MeasurementUnitId)
                .GreaterThan(0).WithMessage("Birim seçimi zorunludur.");

            RuleFor(row => row.Description)
                .MaximumLength(500).WithMessage("Açıklama en fazla 500 karakter olabilir.");
        }
    }
}
