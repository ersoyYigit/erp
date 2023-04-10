using ArdaManager.Application.Requests.Identity;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ArdaManager.Application.Validators.Requests.Identity
{
    public class ForgotPasswordRequestValidator : AbstractValidator<ForgotPasswordRequest>
    {
        public ForgotPasswordRequestValidator(IStringLocalizer<ForgotPasswordRequestValidator> localizer)
        {
            RuleFor(request => request.Email)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Email gereklidir"])
                .EmailAddress().WithMessage(x => localizer["Email hatalı"]);
        }
    }
}