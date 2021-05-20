namespace CMS.Model.Entity
{
    public class MenuItems : BaseModel
    {
        public int MenuId { get; set; }

        public Menu Menu { get; set; }

        public int? ParentId { get; set; }

        public string Label { get; set; }

        public string To { get; set; }

        public int Order { get; set; }

        public bool IsActive { get; set; }

        public bool Deleted { get; set; }
    }
}
