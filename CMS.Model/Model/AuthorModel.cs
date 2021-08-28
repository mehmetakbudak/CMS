using CMS.Model.Entity;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace CMS.Model.Model
{
    public class AuthorModel : BaseModel
    {
        [Required(ErrorMessage = "Yazar adı zorunludur.")]
        public string NameSurname { get; set; }

        public string Resume { get; set; }

        public bool IsActive { get; set; }

        public string FileSrc { get; set; }

        public IFormFile File { get; set; }
    }
}
