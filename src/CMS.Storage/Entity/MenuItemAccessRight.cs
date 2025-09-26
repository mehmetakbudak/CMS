namespace CMS.Storage.Entity
{
    public class MenuItemAccessRight : BaseEntityModel
    {
        public int MenuItemId { get; set; }
        public MenuItem MenuItem { get; set; }
        public int AccessRightId { get; set; }
        public AccessRight AccessRight { get; set; }
    }
}
