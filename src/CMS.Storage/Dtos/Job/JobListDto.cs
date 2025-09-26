using System;

namespace CMS.Storage.Dtos.Job
{
    public class JobListDto
    {
        public int Id { get; set; }
        public int JobLocationId { get; set; }
        public string JobLocationName { get; set; }
        public int TenantId { get; set; }
        public string TenantName { get; set; }
        public string TenantImageUrl { get; set; }
        public string Position { get; set; }
        public string Url { get; set; }
        public int WorkType { get; set; }
        public string WorkTypeName { get; set; } = null;
        public string InsertedUserName { get; set; }
        public DateTime InsertedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
