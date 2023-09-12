using System;

namespace CMS.Storage.Model
{
    public class UserJobModel
    {
        public int Id { get; set; }
        public int JobId { get; set; }
        public string Position { get; set; }
        public string JobLocationName { get; set; }
        public string CompanyName { get; set; }
        public string CompanyImageUrl { get; set; }
        public string WorkTypeName { get; set; }
        public DateTime InsertedDate { get; set; }
    }

    public class UserJobPostModel
    {
        public int UserFileId { get; set; }
        public int JobId { get; set; }
    }
}
