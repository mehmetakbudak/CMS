using Microsoft.AspNetCore.Http;

namespace CMS.Storage.Dtos.Service
{
    public class ServiceDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string ImageUrl { get; set; }
        public string Content { get; set; }
        public bool IsActive { get; set; }
        public IFormFile Image { get; set; }
    }
}
