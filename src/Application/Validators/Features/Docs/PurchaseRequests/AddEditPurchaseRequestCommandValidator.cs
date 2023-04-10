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
    public class AddEditPurchaseRequestCommandValidator : AbstractValidator<AddEditPurchaseRequestCommand>
    {
        public AddEditPurchaseRequestCommandValidator(IStringLocalizer<AddEditCategoryCommandValidator> localizer)
        {
            RuleFor(request => request.DocNo)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Belge numarası gereklidir!"]);


            
            RuleForEach(x => x.PurchaseRequestRowsDto).SetValidator(new PurchaseRequestRowDtoValidator(localizer));
            

            //RuleFor(request => request.WarehouseId).GreaterThan(0).WithMessage(x => localizer["Depo seçimi zorunludur."]);


        }
    }
}
