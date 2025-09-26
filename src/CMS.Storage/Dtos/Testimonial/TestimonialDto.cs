using Microsoft.AspNetCore.Http;

namespace CMS.Storage.Dtos.Testimonial
{
    public class TestimonialDto
    {
        public int Id { get; set; }
        public string CorporateName { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public bool IsActive { get; set; }
        public IFormFile Image { get; set; }
    }
}
