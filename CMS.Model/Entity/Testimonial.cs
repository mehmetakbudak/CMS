namespace CMS.Model.Entity
{
    public class Testimonial : BaseEntityModel
    {
        public string CorporateName { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public bool IsActive { get; set; }
        public bool Deleted { get; set; }
    }
}
