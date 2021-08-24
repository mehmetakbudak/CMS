using CMS.Model.Entity;
using System.ComponentModel.DataAnnotations;

namespace CMS.Model.Model
{
    public class AuthorModel: BaseModel
    {
        [Required(ErrorMessage = "Yazar adı zorunludur.")]
        public string NameSurname { get; set; }

        public string Resume { get; set; }

        public bool IsActive { get; set; }
    }
}
