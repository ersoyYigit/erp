using ArdaManager.Application.Requests.Identity;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ArdaManager.Application.Validators.Requests.Identity
{
    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator(IStringLocalizer<RegisterRequestValidator> localizer)
        {
            RuleFor(request => request.FirstName)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Ad gereklidir"]);
            RuleFor(request => request.LastName)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Soyad gereklidir"]);
            RuleFor(request => request.Email)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Email gereklidir"])
                .EmailAddress().WithMessage(x => localizer["Hatalı email"]);
            /*
            RuleFor(request => request.UserName)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["UserName is required"])
                */
                //.MinimumLength(6).WithMessage(localizer["UserName must be at least of length 6"]);
            RuleFor(request => request.Password)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Parola gereklidir!"])
                .MinimumLength(6).WithMessage(localizer["Parola en az 6 karakter olmalıdır"])
                ;
                //.Matches(@"[A-Z]").WithMessage(localizer["Password must contain at least one capital letter"])
                //.Matches(@"[a-z]").WithMessage(localizer["Password must contain at least one lowercase letter"])
                //.Matches(@"[0-9]").WithMessage(localizer["Password must contain at least one digit"]);
            RuleFor(request => request.ConfirmPassword)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Parola doğrulaması gereklidir!"])
                .Equal(request => request.Password).WithMessage(x => localizer["Parola eşleşmiyor."]);
        }
    }
}