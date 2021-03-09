namespace CMS.Model.Entity
{
    public class RoleAccessRight : BaseModel
    {
        public int RoleId { get; set; }
        public int AccessRightId { get; set; }
        public Role Role { get; set; }
        public AccessRight AccessRight { get; set; }
    }
}
