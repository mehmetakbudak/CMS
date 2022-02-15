using CMS.Model.Entity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CMS.Model.Model
{
    public class UserModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Kullanıcı tipi seçiniz.")]
        public int UserType { get; set; }

        public string UserTypeName { get; set; }

        [Required(ErrorMessage = "Kullanıcı adı zorunludur.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Kullanıcı soyadı zorunludur.")]
        public string Surname { get; set; }

        public string FullName
        {
            get
            {
                return $"{Name} {Surname}";
            }
        }

        [Required(ErrorMessage = "Email adresi zorunludur.")]
        [EmailAddress(ErrorMessage = "Email formatı uygun değildir.")]
        public string EmailAddress { get; set; }

        public bool IsActive { get; set; }
    }

    public class UserProfileModel : BaseModel
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        public string EmailAddress { get; set; }
    }

    public class AddMemberModel
    {
        [Required(ErrorMessage = "Adı zorunludur.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Soyadı zorunludur.")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Email adres zorunludur.")]
        [EmailAddress(ErrorMessage = "Email adresi formatı uygun değildir.")]
        public string EmailAddress { get; set; }

        [Required(ErrorMessage = "Şifre zorunludur.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Şifre yeniden zorunludur.")]
        public string RePassword { get; set; }
    }

    public class ChangePasswordModel
    {
        [Required(ErrorMessage = "Eski şifre zorunludur.")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Mevcut şifre zorunludur.")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Yeni şifre yeniden zorunludur.")]
        public string ReNewPassword { get; set; }
    }

    public class ForgotPasswordModel
    {
        [Required(ErrorMessage = "Email adres zorunludur.")]
        [EmailAddress(ErrorMessage = "Email adresi formatı uygun değildir.")]
        public string EmailAddress { get; set; }
    }

    public class TokenResponseModel
    {
        public string Token { get; set; }

        public string FullName { get; set; }

        public bool IsAccessAdminPanel { get; set; }

        public List<string> OperationAccessRights { get; set; }

        public List<string> MenuAccessRights { get; set; }
    }
}
