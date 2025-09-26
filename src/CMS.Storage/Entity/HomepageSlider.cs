namespace CMS.Storage.Entity
{
    public class HomepageSlider : BaseEntityModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public string ImageUrl { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
        public bool Deleted { get; set; }
    }
}
