namespace CMS.Model.Entity
{
    public class Page : BaseModel
    {
        public int? MenuId { get; set; }
        public Menu Menu { get; set; }
        public string Url { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public bool Published { get; set; }
        public bool IsActive { get; set; }
        public bool Deleted { get; set; }
    }
}
