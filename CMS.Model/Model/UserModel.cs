using CMS.Model.Entity;
using System.ComponentModel.DataAnnotations;

namespace CMS.Model.Model
{
    public class UserModel : BaseModel
    {
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
}
