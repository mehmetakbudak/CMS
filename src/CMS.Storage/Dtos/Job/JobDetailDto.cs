using System;

namespace CMS.Storage.Dtos.Job
{
    public class JobDetailDto
    {
        public int Id { get; set; }
        public string TenantName { get; set; }
        public string TenantImageUrl { get; set; }
        public string TenantWebSiteUrl { get; set; }
        public string TenantTwitterUrl { get; set; }
        public string TenantLinkedinUrl { get; set; }
        public string JobLocationName { get; set; }
        public string WorkTypeName { get; set; }
        public string Position { get; set; }
        public string Description { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsApplyUser { get; set; }
        public DateTime InsertedDate { get; set; }
    }
}
