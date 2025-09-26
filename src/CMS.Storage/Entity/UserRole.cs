namespace CMS.Storage.Entity
{
    public class UserRole : BaseEntityModel
    {
        public int UserId { get; set; }

        public User User { get; set; }

        public int RoleId { get; set; }

        public Role Role { get; set; }
    }
}
