namespace CMS.Model.Entity
{
    public class Service : BaseEntityModel
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string ImageUrl { get; set; }
        public string Content { get; set; }
        public bool IsActive { get; set; }
        public bool Deleted { get; set; }
    }
}
