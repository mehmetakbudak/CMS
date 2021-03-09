using System.ComponentModel.DataAnnotations;

namespace CMS.Model.Model
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Email adresi zorunludur.")]
        [EmailAddress(ErrorMessage = "Email adres formatı uygun değil.")]
        public string EmailAddress { get; set; }

        [Required(ErrorMessage = "Şifre zorunludur.")]
        public string Password { get; set; }

        public string ReturnUrl { get; set; }
    }

    public class LoginReturnModel
    {
        public int UserId { get; set; }
        public int UserType { get; set; }
        public string UserFullName { get; set; }
    }
}
