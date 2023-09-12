using System;

namespace CMS.Storage.Model
{
    public class JobModel
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string CompanyImageUrl { get; set; }
        public string CompanyWebSiteUrl { get; set; }
        public string CompanyTwitterUrl { get; set; }
        public string CompanyLinkedinUrl { get; set; }
        public string JobLocationName { get; set; }
        public string WorkTypeName { get; set; }
        public string Position { get; set; }
        public string Description { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsApplyUser { get; set; }
    }
}
