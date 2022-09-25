namespace CMS.Model.Entity
{
    public class Client : BaseEntityModel
    {
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public bool IsActive { get; set; }
        public bool Deleted { get; set; }
    }
}
