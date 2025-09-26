using CMS.Storage.Dtos.Auth;
using FluentValidation;

namespace CMS.Business.Validations.Auth
{
    public class LoginDtoValidator : AbstractValidator<LoginDto>
    {
        public LoginDtoValidator()
        {
            RuleFor(x => x.EmailAddress)
               .NotEmpty().WithMessage("Email is required.")
               .NotNull().WithMessage("Email is required.")
               .EmailAddress().WithMessage("Email is wrong");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .NotNull().WithMessage("Password is required.");
        }
    }
}