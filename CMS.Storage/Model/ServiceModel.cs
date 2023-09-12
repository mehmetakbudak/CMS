using Microsoft.AspNetCore.Http;

namespace CMS.Storage.Model
{
    public class ServiceModel
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
