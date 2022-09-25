using System.ComponentModel.DataAnnotations;

namespace CMS.Model.Model
{
    public class ContactModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Adı zorunludur.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Soyadı zorunludur.")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Email adresi zorunludur.")]
        [EmailAddress(ErrorMessage = "Email formatı uygun değil.")]
        public string EmailAddress { get; set; }

        [Required(ErrorMessage = "Lütfen konu seçiniz.")]
        public int ContactCategoryId { get; set; }

        [Required(ErrorMessage = "Lütfen mesaj giriniz.")]
        [StringLength(500, ErrorMessage = "Mesaj alanına en fazla 500 karakter girilebilir.")]
        public string Message { get; set; }
    }
}
