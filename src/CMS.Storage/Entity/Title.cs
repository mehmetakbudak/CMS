namespace CMS.Storage.Entity
{
    public class Title : BaseEntityModel
    {        
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public bool Deleted { get; set; }
    }
}
