using System;

namespace CMS.Storage.Entity
{
    public class UserJob : BaseEntityModel
    {
        public int JobId { get; set; }
        public Job Job { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int UserFileId { get; set; }
        public UserFile UserFile { get; set; }
        public DateTime InsertedDate { get; set; }
    }
}
