using System.ComponentModel.DataAnnotations;

namespace CMS.Model.Model
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Email Adresi zorunludur.")]
        [EmailAddress(ErrorMessage = "Email adresi formatı uygun değil")]
        public string EmailAddress { get; set; }

        [Required(ErrorMessage = "Şifre zorunludur.")]
        public string Password { get; set; }
    }
}
