namespace CMS.Model.Entity
{
    public class Team : BaseEntityModel
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public string EmailAddress { get; set; }
        public string Phone { get; set; }
        public string LinkedinUrl { get; set; }
        public string FacebookUrl { get; set; }
        public string TwitterUrl { get; set; }
        public string InstagramUrl { get; set; }
        public bool IsActive { get; set; }
        public bool Deleted { get; set; }
    }
}
