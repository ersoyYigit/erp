using ArdaManager.Application.Features.Docs.PurchaseOffers.Queries;
using ArdaManager.Application.Features.Docs.PurchaseOrders.Queries;
using ArdaManager.Application.Features.Docs.PurchaseRequests.Queries;
using ArdaManager.Application.Validators.Features.Categories.Commands.AddEdit;
using FluentValidation;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Application.Validators.Features.Docs.PurchaseOffer
{
    public class PurchaseOfferRowResponseValidator : AbstractValidator<PurchaseOfferRowResponse>
    {
        public PurchaseOfferRowResponseValidator()
        {
            RuleFor(row => row.ProductCode)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => "Belge numarası gereklidir!");

            RuleFor(row => row.ProductName)
                .NotEmpty().WithMessage("Ürün adı gereklidir!");

            RuleFor(row => row.Quantity)
                .GreaterThan(0).WithMessage("Miktar 0'dan büyük olmalıdır!");
            
            RuleFor(row => row.Price)
                .GreaterThan(0).WithMessage("Fiyat 0'dan büyük olmalıdır!");

            RuleFor(row => row.MeasurementUnitId)
                .GreaterThan(0).WithMessage("Birim seçimi zorunludur.");

            RuleFor(row => row.Description)
                .MaximumLength(500).WithMessage("Açıklama en fazla 500 karakter olabilir.");
        }
    }
}
