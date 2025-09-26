using CMS.Storage.Dtos.Auth;
using FluentValidation;

namespace CMS.Business.Validations.Auth
{
    public class RegisterDtoValidator : AbstractValidator<RegisterDto>
    {
        public RegisterDtoValidator()
        {
            RuleFor(x => x.EmailAddress)
                .NotEmpty().WithMessage("Email is required.")
                .NotNull().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Email is wrong");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .NotNull().WithMessage("Password is required.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .NotNull().WithMessage("Name is required.");

            RuleFor(x => x.Surname)
                .NotEmpty().WithMessage("Surname is required.")
                .NotNull().WithMessage("Surname is required.");
        }
    }
}
