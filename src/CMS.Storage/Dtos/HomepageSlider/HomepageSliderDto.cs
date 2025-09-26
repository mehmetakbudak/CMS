using Microsoft.AspNetCore.Http;

namespace CMS.Storage.Dtos.HomepageSlider
{
    public class HomepageSliderDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public string ImageUrl { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
        public IFormFile Image { get; set; }
    }
}
