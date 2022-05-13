using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace CMS.Model.Model
{
    public class AuthorModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Yazar adı zorunludur.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Yazar soyadı zorunludur.")]
        public string Surname { get; set; }

        public string Resume { get; set; }

        public bool IsActive { get; set; }

        public string FileSrc { get; set; }

        public IFormFile File { get; set; }
    }
}
