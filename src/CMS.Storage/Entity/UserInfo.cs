namespace CMS.Storage.Entity
{
    public class UserInfo : BaseEntityModel
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public int? ReportedUserId { get; set; }
        public User ReportedUser { get; set; }
    }
}
