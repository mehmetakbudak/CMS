using CMS.Model.Model;
using FluentValidation;

namespace CMS.Model.Validation
{
    public class LoginValidation : AbstractValidator<LoginModel>
    {
        public LoginValidation()
        {
            RuleFor(p => p.EmailAddress).NotEmpty().WithMessage("Email adresi alanı boş olamaz.").EmailAddress().WithMessage("Geçerli bir mail adresi giriniz.");
            RuleFor(p => p.Password).NotEmpty().WithMessage("Şifre alanı boş bırakılamaz.");
        }
    }
}
