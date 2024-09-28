using Microsoft.AspNetCore.Http;

namespace CMS.Storage.Model
{
    public class AuthorModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string Resume { get; set; }

        public bool IsActive { get; set; }

        public string FileSrc { get; set; }

        public IFormFile File { get; set; }
    }
}
