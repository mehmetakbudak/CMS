namespace CMS.Storage.Dtos.HomepageSlider
{
    public class HomepageSliderListDto
    {
        public int Id { get; set; }
        public string Title { get; set; }        
        public string Url { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
    }
}
