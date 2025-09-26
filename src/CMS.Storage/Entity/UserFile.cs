using CMS.Storage.Enum;
using System;

namespace CMS.Storage.Entity
{
    public class UserFile : BaseEntityModel
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public string FileName { get; set; }
        public string FileUrl { get; set; }
        public UserFileType FileType { get; set; }
        public bool IsDefault { get; set; }
        public DateTime InsertedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
