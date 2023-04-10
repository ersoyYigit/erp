using ArdaManager.Application.Features.Docs.PurchaseOffers.Commands;
using ArdaManager.Application.Features.Docs.PurchaseRequests.Commands.AddEdit;
using ArdaManager.Application.Validators.Features.Categories.Commands.AddEdit;
using ArdaManager.Application.Validators.Features.Docs.PurchaseRequests;
using FluentValidation;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Application.Validators.Features.Docs.PurchaseOffer
{
    public class UpsertPurchaseOfferCommandValidator : AbstractValidator<UpsertPurchaseOfferCommand>
    {
        public UpsertPurchaseOfferCommandValidator()
        {
            RuleFor(request => request.DocNo)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage("Belge numarası gereklidir!");

            RuleFor(request => request.CompanyId)
                .GreaterThan(0).WithMessage("Tedarikçi firma zorunludur!");



            RuleForEach(x => x.PurchaseOfferRowResponse).SetValidator(new PurchaseOfferRowResponseValidator());


            //RuleFor(request => request.WarehouseId).GreaterThan(0).WithMessage(x => localizer["Depo seçimi zorunludur."]);


        }
    }
}
